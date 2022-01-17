%{
/**
 * Introduction to Compiler Design by Prof. Yi Ping You
 * Project 3 YACC sample
 */
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdarg.h>

#include "header.h"
#include "symtab.h"
#include "semcheck.h"

int yydebug;

extern int linenum;		/* declared in lex.l */
extern FILE *yyin;		/* declared by lex */
extern char *yytext;		/* declared by lex */
extern char buf[256];		/* declared in lex.l */
extern int yylex(void);
extern int tf_count;
extern FILE* outfp;
int yyerror(char* );
__BOOLEAN paramError;			// indicate is parameter have any error?
struct SymTable *symbolTable;	// main symbol table
struct PType *funcReturn;		// record function's return type, used at 'return statement' production rule

int scope = 0;
int var_no=1;
int Opt_D = 1;			/* symbol table dump option */
char fileName[256];
struct insList insList;
char insBuf[256];
int label_count=-1; //count loop label
struct loop_stack loopStack;

struct insList{
	char* list[2048];
	int size;
};
struct loop_stack{
	int stack[100];
	int top;
} ;

// Emit code
void EMIT(char* fmt,...){
	char tmp[256];
	va_list para;
	va_start(para,fmt);
	vsnprintf(tmp,sizeof(tmp),fmt,para);
	va_end(para);
    fprintf(outfp, "%s",tmp);
    fprintf(outfp, "%s","\n");
}
// Delay code emitting
void DELAY(char* fmt,...){
	char tmp[256];

	va_list para;
	va_start(para,fmt);
	vsnprintf(tmp,sizeof(tmp),fmt,para);
	va_end(para);
	insList.list[insList.size++]=strdup(tmp);
	insList.list[insList.size++]=strdup("\n");
}
// Emit Delayed codes
void FORCE(){
	int i;
	for(i=0;i<insList.size;i++){
		fprintf(outfp, "%s",insList.list[i]);
		free(insList.list[i]);
	}
	insList.size=0;
}

// helper for gen code
char* JAVA_VAL_TYPE(SEMTYPE t){
    switch(t) {
        case STRING_t:
            return "Ljava/lang/String;";
        case INTEGER_t:
            return "I";
        case REAL_t:
            return "F";
        case BOOLEAN_t:
            return "Z";
        default:
            return "V";
    }
}
char* JAVA_TYPE_RETURN(SEMTYPE t){
    switch(t) {
        case INTEGER_t:
            return "ireturn";
        case BOOLEAN_t:
            return "ireturn";
        case REAL_t:
            return "freturn";
        default:
            return "return";
    }
}
char funcDecl[501];
char* JAVA_FUNC_DECL(char* id,struct param_sem* params,struct PType* ret){
    struct param_sem *parPtr;
    struct idNode_sem *idPtr;
    memset(funcDecl,0,sizeof(funcDecl));
    snprintf(funcDecl,sizeof(funcDecl),".method public static %s(",id);
    for(parPtr=params;parPtr!=0;parPtr=(parPtr->next))
        for(idPtr=(parPtr->idlist);idPtr!=0;idPtr=(idPtr->next))
            strncat(funcDecl,JAVA_VAL_TYPE(parPtr->pType->type),sizeof(funcDecl)-strlen(funcDecl));
    
    strncat(funcDecl,")",sizeof(funcDecl)-strlen(funcDecl));
    strncat(funcDecl,JAVA_VAL_TYPE(ret->type),sizeof(funcDecl)-strlen(funcDecl));
    return funcDecl;
}
char funcCall[501];
char* JAVA_FUNC_CALL(char* id){
    struct SymNode *node = lookupSymbol( symbolTable, id, 0, __FALSE );
    memset(funcCall,0,sizeof(funcCall));
    snprintf(funcCall,sizeof(funcCall),"invokestatic %s/%s(",fileName,id);
    struct PTypeList* parPtr;
    for( parPtr=node->attribute->formalParam->params; parPtr!=0 ; parPtr=(parPtr->next))
        strncat(funcCall,JAVA_VAL_TYPE(parPtr->value->type),sizeof(funcCall)-strlen(funcCall));
    
    strncat(funcCall,")",sizeof(funcCall)-strlen(funcCall));
    strncat(funcCall,JAVA_VAL_TYPE(node->type->type),sizeof(funcCall)-strlen(funcCall));
    return funcCall;
}
// helper for always delayed codes
void COERCION(struct expr_sem *op1,struct expr_sem *op2) {
	if(op1 && op2)
        if(op1->pType->type==INTEGER_t && op2->pType->type==REAL_t) {
            EMIT("fstore 127");
            EMIT("i2f");
            EMIT("fload 127");
            op1->pType->type = REAL_t;
        }else if( op1->pType->type==REAL_t && op2->pType->type==INTEGER_t){
            EMIT("i2f");
        }
}
void ARITH_OPER(OPERATOR operator,int iOper) {
	switch(operator){
		case ADD_t:
            EMIT(iOper ? "iadd" : "fadd"); break;
		case SUB_t:
            EMIT(iOper ? "isub" : "fsub"); break;
		case MUL_t:
            EMIT(iOper ? "imul" : "fmul"); break;
		case DIV_t:
            EMIT(iOper ? "idiv" : "fdiv"); break;
		case MOD_t:
            EMIT("irem"); break;
	}
}
void ClearExprIns(){
	int i;
	for(i=0;i<insList.size;i++){
		free(insList.list[i]);
	}
	insList.size=0;
}
void GenLoadExpr(struct expr_sem* expr){
	if(expr && expr->varRef){
		struct SymNode*	lookup = lookupLoopVar(symbolTable,expr->varRef->id);
		if(lookup)
            DELAY("iload %d",lookup->attribute->var_no);
		else
		{
			lookup = lookupSymbol(symbolTable,expr->varRef->id,scope,__FALSE);
			if(lookup){
				if(lookup->category==CONSTANT_t)
					switch(expr->pType->type){
						case INTEGER_t:
							DELAY("ldc %d\n",lookup->attribute->constVal->value.integerVal); break;
						case REAL_t:
							DELAY("ldc %lf\n",lookup->attribute->constVal->value.realVal); break;
						case STRING_t:
							DELAY("ldc \"%s\"\n",lookup->attribute->constVal->value.stringVal); break;
						case BOOLEAN_t:
							DELAY("iconst_%d\n",lookup->attribute->constVal->value.booleanVal);break;
					}
				else if((lookup->category==VARIABLE_t ||lookup->category==PARAMETER_t)&& lookup->scope!=0)
					switch(expr->pType->type) {
						case INTEGER_t:
							DELAY("iload %d",lookup->attribute->var_no); break;
						case REAL_t:
							DELAY("fload %d",lookup->attribute->var_no); break;
                        case BOOLEAN_t:
							DELAY("iload %d",lookup->attribute->var_no); break;
                        case STRING_t:
							DELAY("aload %d",lookup->attribute->var_no); break;
                    }
				else if(lookup->category==VARIABLE_t && lookup->scope==0)
                    DELAY("getstatic %s/%s %s",fileName,lookup->name,JAVA_VAL_TYPE(expr->pType->type));
			}
		}
	}
}
void GenSaveExpr(struct expr_sem* expr,struct expr_sem* RHS){
	if(expr->varRef){
		struct SymNode* lookup= lookupSymbol(symbolTable,expr->varRef->id,scope,__FALSE);
		if(lookup && lookup->category==VARIABLE_t){
            if(expr->pType->type == REAL_t && RHS && RHS->pType->type==INTEGER_t)
                DELAY("i2f");
            if(lookup->scope==0)
                DELAY("putstatic %s/%s %s",fileName,lookup->name,JAVA_VAL_TYPE(expr->pType->type));
            else
				switch(expr->pType->type){
					case INTEGER_t:
						DELAY("istore %d",lookup->attribute->var_no); break;
					case REAL_t:
                        DELAY("fstore %d",lookup->attribute->var_no); break;
					case BOOLEAN_t:
						DELAY("istore %d",lookup->attribute->var_no); break;
					case STRING_t:
						DELAY("astore %d",lookup->attribute->var_no); break;
				}
		}
	}
}
void GenPrintStart(){
	DELAY("getstatic java/lang/System/out Ljava/io/PrintStream;");
}
void GenRead(struct expr_sem* expr){
	EMIT("getstatic %s/_sc Ljava/util/Scanner;",fileName);
	if(expr->varRef){
		struct SymNode* lookup= lookupSymbol(symbolTable,expr->varRef->id,scope,__FALSE);
		if(lookup->category==VARIABLE_t)
			switch(expr->pType->type){
				case INTEGER_t:
					EMIT("invokevirtual java/util/Scanner/nextInt()I");
					break;
				case REAL_t:
					EMIT("invokevirtual java/util/Scanner/nextFloat()F");
					break;
				case BOOLEAN_t:
					EMIT("invokevirtual java/util/Scanner/nextBoolean()Z");
					break;
				case STRING_t:
					EMIT("invokevirtual java/util/Scanner/nextLine()Ljava/lang/String;");
					break;
			}
		ClearExprIns();
		GenSaveExpr(expr,NULL);
		FORCE();
	}
}
void GenNegative(SEMTYPE expr){
    DELAY(expr==INTEGER_t ? "ineg" : "fneg");
}
%}

%union {
	int intVal;
	float realVal;
	//__BOOLEAN booleanVal;
	char *lexeme;
	struct idNode_sem *id;
	//SEMTYPE type;
	struct ConstAttr *constVal;
	struct PType *ptype;
	struct param_sem *par;
	struct expr_sem *exprs;
	/*struct var_ref_sem *varRef; */
	struct expr_sem_node *exprNode;
};

/* tokens */
%token ARRAY BEG BOOLEAN DEF DO ELSE END FALSE FOR INTEGER IF OF PRINT READ REAL RETURN STRING THEN TO TRUE VAR WHILE
%token OP_ADD OP_SUB OP_MUL OP_DIV OP_MOD OP_ASSIGN OP_EQ OP_NE OP_GT OP_LT OP_GE OP_LE OP_AND OP_OR OP_NOT
%token MK_COMMA MK_COLON MK_SEMICOLON MK_LPAREN MK_RPAREN MK_LB MK_RB

%token <lexeme>ID
%token <intVal>INT_CONST
%token <realVal>FLOAT_CONST
%token <realVal>SCIENTIFIC
%token <lexeme>STR_CONST

%type<id> id_list
%type<constVal> literal_const
%type<ptype> type scalar_type array_type opt_type
%type<par> param param_list opt_param_list
%type<exprs> var_ref boolean_expr boolean_term boolean_factor relop_expr expr term factor boolean_expr_list opt_boolean_expr_list
%type<intVal> dim mul_op add_op rel_op array_index loop_param

/* start symbol */
%start program
%%

program	: ID
			{
			  struct PType *pType = createPType( VOID_t );
			  struct SymNode *newNode = createProgramNode( $1, scope, pType );
			  insertTab( symbolTable, newNode );
              
              loopStack.top=-1;
              EMIT(".class public %s",fileName);
              EMIT(".super java/lang/Object");
              EMIT(".field public static _sc Ljava/util/Scanner;");
			  if( strcmp(fileName,$1) ) {
				fprintf( stdout, "********** Error at Line#%d: program beginning ID inconsist with file name ********** \n", linenum );
			  }
			}
			  MK_SEMICOLON
			  program_body
			  END ID
			{
			  if( strcmp($1, $6) ) { fprintf( stdout, "********** Error at Line #%d: %s", linenum,"Program end ID inconsist with the beginning ID ********** \n"); }
			  if( strcmp(fileName,$6) ) {
				 fprintf( stdout, "********** Error at Line#%d: program end ID inconsist with file name ********** \n", linenum );
			  }
			  // dump symbol table
			  if( Opt_D == 1 )
				printSymTable( symbolTable, scope );
			}
			;

program_body : opt_decl_list opt_func_decl_list
             {
                EMIT(".method public static main([Ljava/lang/String;)V");
                EMIT(".limit stack 128");
                EMIT(".limit locals 128\n");
                EMIT("new java/util/Scanner");
                EMIT("dup");
                EMIT("getstatic java/lang/System/in Ljava/io/InputStream;");
                EMIT("invokespecial java/util/Scanner/<init>(Ljava/io/InputStream;)V");
                EMIT("putstatic %s/_sc Ljava/util/Scanner;\n",fileName);
             } compound_stmt 
             { 
                EMIT("return");
                EMIT(".end method");
             }
			 ;

opt_decl_list : decl_list
              | /* epsilon */
              ;

decl_list : decl_list decl
          | decl
          ;

decl : VAR id_list MK_COLON scalar_type MK_SEMICOLON       /* scalar type declaration */
     {
      // insert into symbol table
      struct idNode_sem *ptr;
      struct SymNode *newNode;
      for( ptr=$2 ; ptr!=0 ; ptr=(ptr->next) ) {
        if( verifyRedeclaration( symbolTable, ptr->value, scope ) ==__FALSE ) { }
        else {
            if(scope==0){ // global
                newNode = createVarNode( ptr->value, scope, $4,0 );
                EMIT(".field public static %s %s",ptr->value,JAVA_VAL_TYPE($4->type));
            }else{
                newNode = createVarNode( ptr->value, scope, $4,var_no++ );
            }
            insertTab( symbolTable, newNode );
        }
      }

      deleteIdList( $2 );
     }
     | VAR id_list MK_COLON array_type MK_SEMICOLON        /* array type declaration */
     {
      verifyArrayType( $2, $4 );
      // insert into symbol table
      struct idNode_sem *ptr;
      struct SymNode *newNode;
      for( ptr=$2 ; ptr!=0 ; ptr=(ptr->next) ) {
        if( $4->isError == __TRUE ) { }
        else if( verifyRedeclaration( symbolTable, ptr->value, scope ) ==__FALSE ) { }
        else {
            newNode = createVarNode( ptr->value, scope, $4,var_no );
            insertTab( symbolTable, newNode );
        }
      }

      deleteIdList( $2 );
     }
     | VAR id_list MK_COLON literal_const MK_SEMICOLON     /* const declaration */
     {
      struct PType *pType = createPType( $4->category );
      // insert constants into symbol table
      struct idNode_sem *ptr;
      struct SymNode *newNode;
      for( ptr=$2 ; ptr!=0 ; ptr=(ptr->next) ) {
        if( verifyRedeclaration( symbolTable, ptr->value, scope ) ==__FALSE ) { }
        else {
            newNode = createConstNode( ptr->value, scope, pType, $4 );
            insertTab( symbolTable, newNode );
        }
      }

      deleteIdList( $2 );
     }
     ;

literal_const : INT_CONST
              {
                int tmp = $1;
			    $$ = createConstAttr( INTEGER_t, &tmp );
			  }
			  | OP_SUB INT_CONST
			  {
			    int tmp = -$2;
			    $$ = createConstAttr( INTEGER_t, &tmp );
			  }
			  | FLOAT_CONST
			  {
			    float tmp = $1;
			    $$ = createConstAttr( REAL_t, &tmp );
			  }
			  | OP_SUB FLOAT_CONST
			  {
			    float tmp = -$2;
			    $$ = createConstAttr( REAL_t, &tmp );
			  }
			  | SCIENTIFIC
			  {
			    float tmp = $1;
			    $$ = createConstAttr( REAL_t, &tmp );
			  }
			  | OP_SUB SCIENTIFIC
			  {
			    float tmp = -$2;
			    $$ = createConstAttr( REAL_t, &tmp );
			  }
			  | STR_CONST
			  {
			    $$ = createConstAttr( STRING_t, $1 );
			  }
			  | TRUE
			  {
			    __BOOLEAN tmp = __TRUE;
			    $$ = createConstAttr( BOOLEAN_t, &tmp );
			  }
			  | FALSE
			  {
			    __BOOLEAN tmp = __FALSE;
			    $$ = createConstAttr( BOOLEAN_t, &tmp );
			  }
			  ;

opt_func_decl_list : func_decl_list
                   | /* epsilon */
                   ;

func_decl_list : func_decl_list func_decl
               | func_decl
               ;

func_decl : ID MK_LPAREN opt_param_list
          {
			  // check and insert parameters into symbol table
			  var_no=0;
			  paramError = insertParamIntoSymTable( symbolTable, $3, scope+1 );
          } MK_RPAREN opt_type
          {
			  // check and insert function into symbol table
			  if( paramError == __TRUE ) {
			  	printf("--- param(s) with several fault!! ---\n");
			  } else {
				insertFuncIntoSymTable( symbolTable, $1, $3, $6, scope );
                EMIT(JAVA_FUNC_DECL($1,$3,$6));
                EMIT(".limit stack 128");
                EMIT(".limit locals 128");
			  }
			  funcReturn = $6;
          } MK_SEMICOLON compound_stmt
          {
            FORCE();
            EMIT(JAVA_TYPE_RETURN($6->type));
            EMIT(".end method");
          } END ID {
			  if( strcmp($1,$12) ) {
				fprintf( stdout, "********* Error at Line #%d: the end of the functionName mismatch ********** \n", linenum );
			  }
			  funcReturn = 0;
			  var_no=1;
			}
			;

opt_param_list : param_list { $$ = $1; }
               | /* epsilon */ { $$ = 0; }
               ;

param_list : param_list MK_SEMICOLON param
           { 
            param_sem_addParam( $1, $3 );
			$$ = $1;
           }
           | param { $$ = $1; }
           ;

param : id_list MK_COLON type { $$ = createParam( $1, $3 ); }
      ;

id_list	: id_list MK_COMMA ID
        {
            idlist_addNode( $1, $3 );
            $$ = $1;
		}
		| ID { $$ = createIdList($1); }
		;

opt_type : MK_COLON type { $$ = $2; }
         | /* epsilon */ { $$ = createPType( VOID_t ); }
         ;

type : scalar_type { $$ = $1; }
     | array_type { $$ = $1; }
     ;

scalar_type	: INTEGER { $$ = createPType( INTEGER_t ); }
			| REAL { $$ = createPType( REAL_t ); }
			| BOOLEAN { $$ = createPType( BOOLEAN_t ); }
			| STRING { $$ = createPType( STRING_t ); }
			;

array_type		: ARRAY array_index TO array_index OF type
			{
				verifyArrayDim( $6, $2, $4 );
				increaseArrayDim( $6, $2, $4 );
				$$ = $6;
			}
			;

array_index	: INT_CONST { $$ = $1; }
			| OP_SUB INT_CONST { $$ = -$2; }
			;

stmt        : compound_stmt
			| simple_stmt
			| cond_stmt
			| while_stmt
			| for_stmt
			| return_stmt
			| proc_call_stmt
			;

compound_stmt :
			  {
			    scope++;
			  } BEG opt_decl_list opt_stmt_list END
			  {
			    // print contents of current scope
			    if( Opt_D == 1 )
			  	   printSymTable( symbolTable, scope );
			    deleteScope( symbolTable, scope );	// leave this scope, delete...
			    scope--;
			  }
			  ;

opt_stmt_list : stmt_list
			  | /* epsilon */
			  ;

stmt_list : stmt_list stmt
		  | stmt
		  ;

simple_stmt : var_ref OP_ASSIGN boolean_expr MK_SEMICOLON
            {
			  // check if LHS exists
			  __BOOLEAN flagLHS = verifyExistence( symbolTable, $1, scope, __TRUE );
			  // id RHS is not dereferenced, check and deference
			  __BOOLEAN flagRHS = __TRUE;
			  if( $3->isDeref == __FALSE ) {
				flagRHS = verifyExistence( symbolTable, $3, scope, __FALSE );
			  }
			  // if both LHS and RHS are exists, verify their type
			  if( flagLHS==__TRUE && flagRHS==__TRUE )
				verifyAssignmentTypeMatch( $1, $3 );
				GenSaveExpr($1,$3);
				FORCE();
			}
			| PRINT
            {
                FORCE();
                EMIT("getstatic java/lang/System/out Ljava/io/PrintStream;");
            } boolean_expr
            {
                FORCE();
            } MK_SEMICOLON
            {
                verifyScalarExpr( $3, "print" );
                EMIT("invokevirtual java/io/PrintStream/print(%s)V",JAVA_VAL_TYPE($3->pType->type));
            }
 			| READ boolean_expr MK_SEMICOLON
            {
                verifyScalarExpr( $2, "read" );
                GenRead($2);
            }
			;

proc_call_stmt : ID MK_LPAREN opt_boolean_expr_list MK_RPAREN MK_SEMICOLON
               {
                verifyFuncInvoke( $1, $3, symbolTable, scope );
                FORCE();
                EMIT(JAVA_FUNC_CALL($1));
               }
			   ;

cond_stmt : IF condition THEN opt_stmt_list
          {
            FORCE();
            EMIT("goto Lcondexit_%d\nLfalse_%d:",loopStack.stack[loopStack.top],loopStack.stack[loopStack.top]);
          } ELSE opt_stmt_list
          {
            FORCE();
            EMIT("Lcondexit_%d:",loopStack.stack[loopStack.top]);
            loopStack.top--;
          } END IF
          | IF condition THEN opt_stmt_list
          {
			FORCE();
			EMIT("Lfalse_%d:",loopStack.stack[loopStack.top]);
          } END IF
          {
			loopStack.top--;
          }
          ;

condition : boolean_expr
          {
			loopStack.top++;
			label_count++;
			loopStack.stack[loopStack.top]=label_count;

		    verifyBooleanExpr( $1, "if" );
			FORCE();
		    EMIT("ifeq Lfalse_%d",loopStack.stack[loopStack.top]);
		  }
          ;

while_stmt : WHILE
           {
			loopStack.top++;
			label_count++;
			loopStack.stack[loopStack.top]=label_count; //push to stack
			FORCE();
			EMIT("Lbegin_%d:",loopStack.stack[loopStack.top]);
		   } condition_while
           {
			FORCE();
			EMIT("ifeq Lexit_%d",loopStack.stack[loopStack.top]);
		   } DO opt_stmt_list
           {
			  FORCE();
			  EMIT("goto Lbegin_%d\nLexit_%d:",loopStack.stack[loopStack.top],loopStack.stack[loopStack.top]);
           } END DO {loopStack.top--;}
           ;

condition_while	: boolean_expr { verifyBooleanExpr( $1, "while" ); }
                ;

for_stmt : FOR ID
         {
           insertLoopVarIntoTable( symbolTable, $2 );
         } OP_ASSIGN loop_param TO loop_param
         {
           verifyLoopParam( $5, $7 );
           loopStack.top++, label_count++;
           struct SymNode* ptr=lookupLoopVar(symbolTable,$2);
           if(ptr){
               loopStack.stack[loopStack.top]=label_count; //push to stack

                int l_index = loopStack.stack[loopStack.top],addr = ptr->attribute->var_no;
                FORCE();
                EMIT("sipush %d",$5);
                EMIT("istore %d",addr);
                EMIT("Lbegin_%d:",l_index);
                EMIT("iload %d",addr);
                EMIT("sipush %d",$7+1);
                EMIT("isub");
                EMIT("iflt Ltrue_%d",l_index);
                EMIT("iconst_0");
                EMIT("goto Lfalse_%d",l_index);
                EMIT("Ltrue_%d:",l_index);
                EMIT("iconst_1");
                EMIT("Lfalse_%d:",l_index);
                EMIT("ifeq Lexit_%d",l_index);
           }
         } DO opt_stmt_list END DO
         {
           FORCE();
           struct SymNode* ptr;
           ptr=lookupLoopVar(symbolTable,$2);
           if(ptr){
                int l_index = loopStack.stack[loopStack.top],addr = ptr->attribute->var_no;
                EMIT("iload %d",addr);
                EMIT("sipush 1");
                EMIT("iadd");
                EMIT("istore %d",addr);
                EMIT("goto Lbegin_%d",l_index);
                EMIT("Lexit_%d:",l_index);
           }
           loopStack.top--;
           popLoopVar( symbolTable );
         }
         ;

loop_param : INT_CONST { $$ = $1; }
           | OP_SUB INT_CONST { $$ = -$2; }
           ;

return_stmt : RETURN boolean_expr MK_SEMICOLON
			{
			  verifyReturnStatement( $2, funcReturn );
			  FORCE();
			}
			;

opt_boolean_expr_list : boolean_expr_list { $$ = $1; }
			          | /* epsilon */ { $$ = 0; }	// null
			          ;

boolean_expr_list : boolean_expr_list MK_COMMA boolean_expr
                  {
                    struct expr_sem *exprPtr;
                    for( exprPtr=$1 ; (exprPtr->next)!=0 ; exprPtr=(exprPtr->next) );
                    exprPtr->next = $3;
                    $$ = $1;
                  }
                  | boolean_expr
                  {
                    $$ = $1;
                  }
                  ;

boolean_expr : boolean_expr OP_OR boolean_term
			{
			  verifyAndOrOp( $1, OR_t, $3 );
              FORCE();
			  EMIT("ior");
			  $$ = $1;
			}
			| boolean_term { $$ = $1; }
			;

boolean_term : boolean_term OP_AND boolean_factor
			{
			  verifyAndOrOp( $1, AND_t, $3 );
              FORCE();
			  EMIT("iand");
			  $$ = $1;
			}
			| boolean_factor { $$ = $1; }
			;

boolean_factor : OP_NOT boolean_factor
			{
			  verifyUnaryNOT( $2 );
              FORCE();
			  EMIT( "iconst_1"),EMIT("ixor");
			  $$ = $2;
			}
			| relop_expr { $$ = $1; }
			;

relop_expr : expr rel_op expr
           {
            //GenRelational($1,$2,$3);
            loopStack.top++, label_count++;
            loopStack.stack[loopStack.top]=label_count;
            int addr = loopStack.stack[loopStack.top];
            if($1->pType->type == INTEGER_t)
                DELAY("isub");
            else if($1->pType->type == REAL_t)
                DELAY("fcmpl");
            switch($2){
                case LT_t:
                    DELAY("iflt Ltrue_%d",addr); break;
                case LE_t:
                    DELAY("ifle Ltrue_%d",addr); break;
                case NE_t:
                    DELAY("ifne Ltrue_%d",addr); break;
                case GE_t:
                    DELAY("ifge Ltrue_%d",addr); break;
                case GT_t:
                    DELAY("ifgt Ltrue_%d",addr); break;
                case EQ_t:
                    DELAY("ifeq Ltrue_%d",addr); break;
            }
            DELAY("iconst_0");
            DELAY("goto LExit_Rel_%d",addr);
            DELAY("Ltrue_%d:",addr);
            DELAY("iconst_1");
            DELAY("LExit_Rel_%d:",addr);
            loopStack.top--;
            verifyRelOp( $1, $2, $3 );
            $$ = $1;
           }
           | expr { $$ = $1; }
           ;

rel_op : OP_LT { $$ = LT_t; }
       | OP_LE { $$ = LE_t; }
       | OP_EQ { $$ = EQ_t; }
       | OP_GE { $$ = GE_t; }
       | OP_GT { $$ = GT_t; }
       | OP_NE { $$ = NE_t; }
       ;

expr : expr add_op term
     {
        FORCE();
        COERCION($1,$3);
        ARITH_OPER($2,$1->pType->type == INTEGER_t);
		verifyArithmeticOp( $1, $2, $3 );
		$$ = $1;
     }
     | term { $$ = $1; }
     ;

add_op : OP_ADD { $$ = ADD_t; }
       | OP_SUB { $$ = SUB_t; }
       ;

term : term mul_op factor
     {
        FORCE();
        COERCION($1,$3);
        ARITH_OPER($2,$1->pType->type == INTEGER_t);
	    if( $2 == MOD_t ) {
			verifyModOp( $1, $3 );
		}
		else {
			verifyArithmeticOp( $1, $2, $3 );
		}
		$$ = $1;
     }
	 | factor { $$ = $1;  }
     ;

mul_op : OP_MUL { $$ = MUL_t; }
       | OP_DIV { $$ = DIV_t; }
       | OP_MOD { $$ = MOD_t; }
       ;

factor : var_ref
       {
          verifyExistence( symbolTable, $1, scope, __FALSE );
          GenLoadExpr($1);
          $$ = $1;
          $$->beginningOp = NONE_t;
       }
       | OP_SUB var_ref
       {
          verifyExistence( symbolTable, $2, scope, __FALSE );
          verifyUnaryMinus( $2 );
          $$ = $2;
          $$->beginningOp = SUB_t;
          GenLoadExpr($2);
          GenNegative($2->pType->type);
       }
       | MK_LPAREN boolean_expr MK_RPAREN
       {
          $2->beginningOp = NONE_t;
          $$ = $2;
       }
       | OP_SUB MK_LPAREN boolean_expr MK_RPAREN
       {
          verifyUnaryMinus( $3 );
          $$ = $3;
          $$->beginningOp = SUB_t;
          GenNegative($3->pType->type);
       }
       | ID MK_LPAREN opt_boolean_expr_list MK_RPAREN
       {
          $$ = verifyFuncInvoke( $1, $3, symbolTable, scope );
          $$->beginningOp = NONE_t;
          FORCE();
          EMIT(JAVA_FUNC_CALL($1));
       }
       | OP_SUB ID MK_LPAREN opt_boolean_expr_list MK_RPAREN
       {
          $$ = verifyFuncInvoke( $2, $4, symbolTable, scope );
          $$->beginningOp = SUB_t;
          FORCE();
          EMIT(JAVA_FUNC_CALL($2));
          GenNegative(lookupSymbol( symbolTable, $2, 0, __FALSE )->type->type);
       }
       | literal_const
       {
          $$ = (struct expr_sem *)malloc(sizeof(struct expr_sem));
          $$->isDeref = __TRUE;
          $$->varRef = 0;
          $$->pType = createPType( $1->category );
          $$->next = 0;
          if( $1->hasMinus == __TRUE ) {
            $$->beginningOp = SUB_t;
          }
          else {
            $$->beginningOp = NONE_t;
          }
           switch($1->category) {
               case STRING_t:
                   DELAY("ldc \"%s\"",$1->value.stringVal); break;
               case INTEGER_t:
                   DELAY("ldc %d",$1->value.integerVal); break;
               case REAL_t:
                   DELAY("ldc %lf",$1->value.realVal); break;
               case BOOLEAN_t:
                   DELAY("iconst_%d",$1->value.booleanVal); break;
           }
       }
       ;

var_ref : ID
        {
            $$ = createExprSem( $1 );
		}
		| var_ref dim
		{
			increaseDim( $1, $2 );
			$$ = $1;
		}
		;

dim	: MK_LB boolean_expr MK_RB
    {
        $$ = verifyArrayIndex( $2 );
    }
    ;

%%

int yyerror( char *msg )
{
	(void) msg;
	fprintf( stderr, "\n|--------------------------------------------------------------------------\n" );
	fprintf( stderr, "| Error found in Line #%d: %s\n", linenum, buf );
	fprintf( stderr, "|\n" );
	fprintf( stderr, "| Unmatched token: %s\n", yytext );
	fprintf( stderr, "|--------------------------------------------------------------------------\n" );
	exit(-1);
}


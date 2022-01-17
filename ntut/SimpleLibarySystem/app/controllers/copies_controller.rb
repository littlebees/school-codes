class CopiesController < ApplicationController
    load_and_authorize_resource :book
    load_and_authorize_resource :through => :book

    def index
#        @copies = @book.copies
    end 
    
    def show
#        @copy = @book.copies.find( params[:id] )
    end

    def new
#        @copy = @book.copies.new
    end

    def create
#        @copy = @book.copies.new( copy_params )
        if @copy.save
            redirect_to book_copies_url(@book)
        else
            render :action => :new
        end
    end

    def edit
#        @copy = @book.copies.find( params[:id] )
    end

    def update
#        @copy = @book.copies.find( params[:id] )
        if @copy.update( copy_params )
            redirect_to book_copies_url(@book)
        else
            render :action => :edit
        end
    end

    def destroy
#        @copy = @book.copies.find( params[:id] )
        @copy.destroy
        @book.copy_number = @book.copy_number - 1

        redirect_to book_copies_url(@book)
    end
        
        
    protected
    def find_Book
        @book = Book.find params[:book_id]
    end

    def copy_params
           params.require(:copy).permit(:state,:enter_time)
    end
end

class BooksController < ApplicationController
    load_and_authorize_resource 

    def index
#        @books = Book.all
        if params[:search]
            @books = Book.simple_search params.permit(:search)[:search]
        end
    end

    def show
#        @book = Book.find params[:id]
        @copies = @book.copies
    end

    def new
#        @book = Book.new
    end

    def create
#        @book = Book.new(book_params)
        @book.save
        add_authors(params[:authorstr],params[:authororg])
        add_publisher(params[:publishname],params[:publishaddr])
        redirect_to books_url
    end

    def edit
#        @book = Book.find(params[:id])
        session[:edit_book_id] = params[:id]
    end

    def update
#        @book = Book.find(params[:id])
        

        if @book.update(book_params)
            redirect_to book_url(@book)
        else
            render :action => :edit
        end
    end

    def destroy
#        @book = Book.find(params[:id])
        @book.destroy

        redirect_to books_url
    end


    def appoint
        ## 找出有無空的copy
        ## 再新建bill
#        @book = Book.find(params[:id])
        if current_user
            copy = @book.find_not_booked_copy
            unless copy.nil?
                if Bill.create(:copy => copy,:user => current_user)
                    flash[:notice] = "預約成功"
                else
                    flash[:alert] = "預約失敗"
                end
            else
                flash[:alert] = "沒書了"
            end
        else
                flash[:alert] = "oops"
        end
            redirect_to books_url
    end

    def add_copy
        if (@book.copies << Copy.new)
            flash[:notice] = "新增副本成功"
        else
            flash[:alert] = "新增副本失敗"
        end
        redirect_to book_url(@book)
    end

    protected
    def book_params
        params.require(:book).permit(:isbn,:title,:year,:copy_number,:category)
    end
    
    def add_authors(str,orgs)
        au = (str.split(';')).zip(orgs.split(';')).map do |s,o| 
            Author.where(:name => s).first || Author.create(:name => s, :organization => (o.blank? ? nil : o))
        end
        au.each do |a|
            @book.authors << a
        end
    end

    def add_publisher(name,addr)
        publisher = Publisher.where(:name => name).first || Publisher.create(:name => name,:address => addr)
        @book.publisher = publisher
    end

end

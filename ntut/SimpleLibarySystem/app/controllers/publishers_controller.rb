class PublishersController < ApplicationController
    load_and_authorize_resource
    def show
#        @publisher = Publisher.find params[:id]
        @books = @publisher.books
    end

    def edit
#        @publisher = Publisher.find params[:id]
    end

    def update
#        @publisher = Publisher.find params[:id]

#        if session[:edit_book_id]
        if @publisher.update ( publisher_parmas )
            redirect_to publisher_url(@publisher)
#             redirect_to book_url(session[:edit_book_id])
        else
            render :action => :edit
        end
#        else
#            redirect_to root_url
#        end
    end

    protected
    def publisher_parmas
        params.require(:publisher).permit(:name,:address)
    end
end

class Librarian::BillsController < Librarian::BaseController
load_and_authorize_resource
    def index
#        @bills = Bill.all.order("created_at DESC")
         @bills = @bills.order("created_at DESC")
    end

    def show
#        @bill = Bill.find params[:id]
    end

    def update
#        @bill = Bill.find params[:id]
        if @bill.agree_to_book_the_copy && @bill.save
          redirect_to librarian_bills_url
        else
          # add error content
          redirect_to librarian_bills_url
        end
    end

    def destroy
#        @bill = Bill.find params[:id]
        @bill.destroy 
        redirect_to librarian_bills_url
    end
end

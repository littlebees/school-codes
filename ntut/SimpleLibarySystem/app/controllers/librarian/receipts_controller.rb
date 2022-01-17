class Librarian::ReceiptsController < Librarian::BaseController
    load_and_authorize_resource
    def index
#    @receipts = Receipt.all.order("created_at DESC")
#        @receipts = @receipts.order("created_at DESC")
        @receipts = Receipt.all
    end
    
    def show
#        @receipt = Receipt.find parmas[:receipt_id]
    end
    def update
#        @receipt = Receipt.find parmas[:receipt_id]
        
        if @receipt.return_the_copy && @receipt.save
            redirect_to librarian_receipts_url
        else
            # add error content
            redirect_to librarian_receipts_url
        end
    end
    def destroy
#        @receipt = Receipt.find params[:receipt_id]
        @receipt.destroy

        redirect_to librarian_receipts_url
    end
end

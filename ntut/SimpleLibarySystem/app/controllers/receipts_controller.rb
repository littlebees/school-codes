class ReceiptsController < ApplicationController
    load_and_authorize_resource :through => :current_user

#    def index
#        @receipts = current_user.receipts
#    end
#
#    def show
#        @receipt = current_user.receipts.find parmas[:id]
#    end

    def update
#	 @receipt = current_user.receipts.find params[:id]
        if @receipt.return_the_copy
            flash[:notice] = "還書成功"
        else
            flash[:alert] = "還書失敗"
        end
        redirect_to user_url(current_user)
   end

end

class BillsController < ApplicationController
    load_and_authorize_resource :through => :current_user
#    def index
#        @bills = current_user.bills.order("created_at DESC")
#    end

#    def show
#        @bill = current_user.bills.find params[:id]
#    end

    def update
#        @bill = current_user.bills.find params[:id]
        if @bill.agreed? 
            if @bill.takes_the_copy
                flash[:notice] = "取書成功"
            else
                flash[:alert] = "取書失敗"
            end
        else
            ## can use render??
            flash[:alert] = "請等待館員確認"
        end
        redirect_to user_url(current_user)
    end

    def destroy
#        @bill = current_user.bills.find params[:id]
        if @bill.destroy
            flash[:notice] = "取消成功"
        else
            flash[:alert] = "取消失敗"
        end
        redirect_to user_url(current_user)
    end

end

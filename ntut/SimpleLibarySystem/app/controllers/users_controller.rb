class UsersController < ApplicationController
    load_and_authorize_resource 
    def show
#        @user = current_user
        @bills = current_user.bills
        @receipts = current_user.receipts
    end
    def index
        redirect_to root_path 
    end 
end

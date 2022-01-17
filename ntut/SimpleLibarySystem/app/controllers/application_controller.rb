class ApplicationController < ActionController::Base
  include CanCan::ControllerAdditions

  # Prevent CSRF attacks by raising an exception.
  # For APIs, you may want to use :null_session instead.
  protect_from_forgery with: :exception
  before_action :configure_permitted_parameters,if: :devise_controller?

  rescue_from CanCan::AccessDenied do |exception|
    if current_user.nil?
        redirect_to new_user_session_url, :alert => "You have to log in to continue."
    else
        redirect_to books_url, :alert => "access denied"
    end
  end

  protected
  def configure_permitted_parameters
    devise_parameter_sanitizer.permit(:sign_up, keys: [:name])
    devise_parameter_sanitizer.permit(:account_update, keys: [:name])
  end
end

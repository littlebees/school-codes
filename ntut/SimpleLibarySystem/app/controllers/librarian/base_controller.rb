class Librarian::BaseController < ApplicationController
    def current_ability
      @current_ability ||= LibrarianAbility.new(current_user)
    end
end

class Ability
  include CanCan::Ability

  def initialize(user)
    if user.blank?
        basic_read_only
    elsif user.has_role?(:admin)
        basic_read_only
        can :manage, User
    elsif user.has_role?(:librarian)
        basic_read_only
        book_and_borrow(user)
        can :manage, Book
        can :manage, Copy
        can :manage, Author
        can :manage, Publisher
    elsif user.has_role?(:reader)
        basic_read_only
        book_and_borrow(user)
        can :appoint, Book
    else
        basic_read_only
    end
    # Define abilities for the passed in user here. For example:
    #
    #   user ||= User.new # guest user (not logged in)
    #   if user.admin?
    #     can :manage, :all
    #   else
    #     can :read, :all
    #   end
    #
    # The first argument to `can` is the action you are giving the user
    # permission to do.
    # If you pass :manage it will apply to every action. Other common actions
    # here are :read, :create, :update and :destroy.
    #
    # The second argument is the resource the user can perform the action on.
    # If you pass :all it will apply to every resource. Otherwise pass a Ruby
    # class of the resource.
    #
    # The third argument is an optional hash of conditions to further filter the
    # objects.
    # For example, here the user can only update published articles.
    #
    #   can :update, Article, :published => true
    #
    # See the wiki for details:
    # https://github.com/CanCanCommunity/cancancan/wiki/Defining-Abilities
  end
  private
  def basic_read_only
    can :read ,Book
    can :show, Copy
    can :show, Author
    can :show, Publisher
  end
  def book_and_borrow(user)
        can :show, User do |u|
            (u.id == user.id)
        end
        can :update, Bill do |b|
            (b.user_id == user.id)
        end
        can :destroy, Bill do |b|
            (b.user_id == user.id)
        end
        can :update, Receipt do |r|
            (r.user_id == user.id)
        end

  end
end

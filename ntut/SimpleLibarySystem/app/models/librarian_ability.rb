class LibrarianAbility
  include CanCan::Ability
    def initialize(user)
        if user.has_role? :librarian
            can :manage,:all
        else
            cannot :manage,:all
        end
    end
end

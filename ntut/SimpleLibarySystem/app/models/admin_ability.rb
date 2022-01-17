class AdminAbility
  include CanCan::Ability
    def initialize(user)
        unless user.has_role? :admin
            cannot :manage,:all
        else
            can :manage,:all
        end
    end
end

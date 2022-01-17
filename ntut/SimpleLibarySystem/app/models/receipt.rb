class Receipt < ApplicationRecord
  belongs_to :user
  belongs_to :copy

    INTERNAL = 30
    def borrows_the_copy
        unless self.lent_out_date
            self.lent_out_date = Date.today
            self.due_date = self.lent_out_date + INTERNAL
            self.copy.state = 2
            self.copy.save
            return true
        end
        return false
    end

    def return_the_copy
        unless self.back_date
            self.back_date = Date.today
            self.copy.state = 0
            self.copy.save
            self.save
            return true
        end
        return false
    end

    def out_of_date?
        Date.today > self.due_date
    end

    def borrowed?
        !self.lent_out_date.nil?
    end
end

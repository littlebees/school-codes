class Bill < ApplicationRecord
  belongs_to :user
  belongs_to :copy
  INTERNAL = 10

  def agree_to_book_the_copy
    unless self.reserve_time
        self.reserve_time = Date.today
        self.due_time = self.reserve_time + INTERNAL
        self.copy.state = 1
        self.copy.save
        return true
    end
    return false
  end

  def out_of_date?
    Date.today > self.due_time
  end

  def agreed?
    !self.reserve_time.nil?
  end   

  def takes_the_copy
    self.take_date = Date.today
    if self.save
        a = self.copy.build_receipt(:user_id => self.user_id)
        a.borrows_the_copy
        if a.save
            true
        else
            false
        end
    else
        false
    end
  end

  def taked?
    !self.take_date.nil?
  end
end

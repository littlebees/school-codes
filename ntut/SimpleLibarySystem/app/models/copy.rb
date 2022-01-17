class Copy < ApplicationRecord
    belongs_to :book
    has_one :receipt, ->{ where(:back_date => nil) }
    has_one :bill, ->{ where(:take_date => nil) }
    has_one :user, through: :receipt

    before_save :check_state_and_enter_time
    after_create :plus_copy_number

    enum state: [:available,:booked,:borrowed]

    protected
    def check_state_and_enter_time
        self.state ||= 0;
        self.enter_time ||= Date.today;
    end

    def plus_copy_number
        self.book.copy_number = self.book.copy_number + 1
        self.book.save
    end
end

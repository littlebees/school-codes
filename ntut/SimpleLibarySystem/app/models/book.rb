class Book < ApplicationRecord
    has_many :copies, dependent: :destroy
    has_many :booksAuthors
    has_many :authors, through: :booksAuthors
    has_one  :bookPublisher
    has_one  :publisher, through: :bookPublisher
    

    attr_accessor :authorstr
    attr_accessor :authororg
    attr_accessor :publishname
    attr_accessor :publishaddr

    before_save :check_copy_number
    after_create  :add_copies
    
    def find_not_booked_copy
        self.copies.each do |c|
            if c.state == "available" && c.bill.nil?
                return c
            end
        end
        return nil
    end

    def self.simple_search(search)
        where('title LIKE ?', "%#{search}%")
    end

    protected
    def check_copy_number
        self.copy_number ||= 1
    end

    def add_copies
        copy_number = self.copy_number
        copies_size = self.copies.size
        self.copy_number = 0;
        self.save
        if copy_number > copies_size
            (copy_number - copies_size).times do 
                self.copies.create
            end
        end
    end
end

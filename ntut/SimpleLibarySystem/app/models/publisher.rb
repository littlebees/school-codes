class Publisher < ApplicationRecord
    has_many :bookPublishers
    has_many :books, through: :bookPublishers
end

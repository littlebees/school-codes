class Author < ApplicationRecord
    has_many :booksAuthors
    has_many :books, through: :booksAuthors
end

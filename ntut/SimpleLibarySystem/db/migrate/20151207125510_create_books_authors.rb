class CreateBooksAuthors < ActiveRecord::Migration[5.0]
  def change
    create_table :books_authors do |t|
      t.belongs_to :book, index: true
      t.belongs_to :author, index: true

      t.timestamps null: false
    end
    add_foreign_key :books_authors, :books
    add_foreign_key :books_authors, :authors
  end
end

class CreateBookPublishers < ActiveRecord::Migration[5.0]
  def change
    create_table :book_publishers do |t|
      t.belongs_to :book, index: true
      t.belongs_to :publisher, index: true

      t.timestamps null: false
    end
    add_foreign_key :book_publishers, :books
    add_foreign_key :book_publishers, :publishers
  end
end

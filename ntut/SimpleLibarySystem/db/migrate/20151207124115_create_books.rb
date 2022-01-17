class CreateBooks < ActiveRecord::Migration[5.0]
  def change
    create_table :books do |t|
      t.string :isbn, null:false
      t.string :title, null:false
      t.date :year, null:false
      t.integer :copy_number, null:false, default: 0
      t.string :category

      t.timestamps null: false
    end
  end
end

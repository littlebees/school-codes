class CreateCopies < ActiveRecord::Migration[5.0]
  def change
    create_table :copies do |t|
      t.belongs_to :book, null:false
      t.date :enter_time, null:false
      t.integer :state, null:false

      t.timestamps null: false
    end
  end
end

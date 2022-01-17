class CreateBills < ActiveRecord::Migration[5.0]
  def change
    create_table :bills do |t|
      t.belongs_to :user, index: true
      t.belongs_to :copy, index: true
      t.date :reserve_time
      t.date :due_time
      t.date :take_date

      t.timestamps null: false
    end
    add_foreign_key :bills, :users
    add_foreign_key :bills, :copies
  end
end

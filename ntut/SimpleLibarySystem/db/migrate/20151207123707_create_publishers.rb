class CreatePublishers < ActiveRecord::Migration[5.0]
  def change
    create_table :publishers do |t|
      t.string :name, null:false
      t.string :address, null:false

      t.timestamps null: false
    end
  end
end

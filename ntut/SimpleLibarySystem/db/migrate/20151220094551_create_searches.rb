class CreateSearches < ActiveRecord::Migration[5.0]
  def change
    create_table :searches do |t|
      t.string :keyword
      t.string :authors
      t.integer :publisher_id

      t.timestamps null: false
    end
  end
end

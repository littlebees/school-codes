class ChangeBookDefaultToNull < ActiveRecord::Migration[5.0]
  def change
  change_column_default :books, :copy_number, nil
  end
end

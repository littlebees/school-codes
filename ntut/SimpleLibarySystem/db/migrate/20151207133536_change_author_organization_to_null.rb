class ChangeAuthorOrganizationToNull < ActiveRecord::Migration[5.0]
  def change
    change_column_null :authors, :organization, true
  end
end

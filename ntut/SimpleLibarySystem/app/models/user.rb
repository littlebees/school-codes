class User < ApplicationRecord
  rolify
  # Include default devise modules. Others available are:
  # :confirmable, :lockable, :timeoutable and :omniauthable
  devise :database_authenticatable, :registerable,
         :recoverable, :rememberable, :trackable, :validatable
  after_create :add_reader
  has_many :receipts
  has_many :bills
  has_many :copies, through: :receipts
  protected
  def add_reader
    self.add_role(:reader) if self.roles.blank?
  end
end

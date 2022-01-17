# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)

Data_num = 10
Centries = (950..990).to_a + (1..9).to_a
Orgs = ["Comic","Wiki","Comp","Ruby"]
Addrs = ["Taiwan","US","Earth","Base","Foot","Whatever!!"]

def split_int x
    if x/10 == 0
        [x]
    else
        split_int(x/10) << x%10
    end
end

def make_isbn
    myList = [1,3].cycle
    isbn = Array.new 4
    isbn[0] = 978
    isbn[1] = Centries.sample
    isbn[2] = (0..300).to_a.sample
    isbn[3] = (10..1000).to_a.sample
    isbn << 10 - (isbn.inject([]) {|a,b| a + split_int(b) }).inject(0){ |a,b| a + b*myList.next } % 10 
    isbn.join('-')
end

authors = (1..Data_num).inject([]) do |a,b|
                a << Author.create(name: "author#{b}", organization: (Orgs.sample))
            end
publishers = (1..Data_num).inject([]) do |a,b|
                a << Publisher.create(name: "publisher#{b}", address: (Addrs.sample))
            end
books = (1..Data_num).inject([]) do |a,b|
                a << Book.create(title:"book#{b}", year: (Date.today), isbn: (make_isbn))
            end

books.each do |b|
    b.publisher = publishers.sample
    b.authors << authors.sample(1+rand(Data_num))
end

password = "102331020"
users = (1..Data_num).inject([]) do |a,b|
                    a << User.create(name:"user#{b}", email:"user#{b}@user.com", password: password)
                end
admin = User.create name: "admin",email: "admin@user.com",password: password
librarian = User.create name: "librarian",email: "librarian@user.com",password: password
admin.add_role :admin
librarian.add_role :librarian

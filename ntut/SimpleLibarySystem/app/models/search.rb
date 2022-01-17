class Search < ApplicationRecord

def books
  @books ||= find_books
end
def find_books
#Book.joins(:authors).joins(:publisher).where(conditions)
    unless authors.blank?
        authors_cond
    else
        Book.eager_load(:authors).eager_load(:publisher).where(conditions)
    end       
end

private

def parse_authors
    arr = authors.split(';')
    [arr,arr.size]
end

def author_cond(name)
    ["authors.name = ?",name]
end


def keyword_conditions
  ["books.title LIKE ?", "%#{keyword}%"] unless keyword.blank?
end

def authors_cond
    authors,num = parse_authors
    Book.eager_load(:authors).eager_load(:publisher).where(conditions).where('authors.name IN (?)',authors).group('books.id').having('COUNT(DISTINCT authors.id) >= ?',num)
end

def publisher_conditions
  ["publishers.id = ?", publisher_id] unless publisher_id.blank?
end

def conditions
  [conditions_clauses.join(' AND '), *conditions_options]
end

def conditions_clauses
  conditions_parts.map { |condition| condition.first }
end

def conditions_options
  conditions_parts.map { |condition| condition[1..-1] }.flatten
end

def conditions_parts
  private_methods(false).grep(/_conditions$/).map { |m| send(m) }.compact
end
end

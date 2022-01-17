json.array!(@searches) do |search|
  json.extract! search, :id, :keyword, :authors, :publisher_id
  json.url search_url(search, format: :json)
end

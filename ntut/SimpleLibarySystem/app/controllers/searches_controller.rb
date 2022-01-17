class SearchesController < ApplicationController
  before_action :set_search, only: [:show]

  # GET /searches/1
  # GET /searches/1.json
  def show
    @books = @search.find_books
  end

  # GET /searches/new
  def new
    @search = Search.new
  end


  # POST /searches
  # POST /searches.json
  def create
    @search = Search.new(search_params)

    respond_to do |format|
      if @search.save
        format.html { redirect_to @search }
        format.json { render :show, status: :created, location: @search }
      else
        format.html { render :new }
        format.json { render json: @search.errors, status: :unprocessable_entity }
      end
    end
  end


  private
    # Use callbacks to share common setup or constraints between actions.
    def set_search
      @search = Search.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def search_params
      params.require(:search).permit(:keyword, :authors, :publisher_id)
    end
end

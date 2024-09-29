namespace KH.BuildingBlocks.Responses
{
  public class PagedResponse<T> where T : class
  {
    public PagedResponse(IList<T> items,
        int currentPage = 1,
        int totalPages = 0,
        int pageSize = 10,
        int totalCount = 0)
    {
      CurrentPage = currentPage;
      TotalPages = totalPages;
      PageSize = pageSize;
      TotalCount = totalCount;
      Items = items;
    }
    public IList<T> Items { get; set; }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }



  }


}

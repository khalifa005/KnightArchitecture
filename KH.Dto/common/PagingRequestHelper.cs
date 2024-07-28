namespace KH.Dto.common
{
  public class PagingRequestHelper
  {
    public const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
      get => _pageSize;
      set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string? Sort { get; set; }
    private string _search { get; set; }
    public string? Search
    {
      get => _search;
      set => _search = value.ToLower();
    }
  }
}

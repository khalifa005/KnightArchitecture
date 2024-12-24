namespace KH.BuildingBlocks.Apis.Responses;

public class PagedList<T> where T : class // Enforce T to be a reference type
{
  public int CurrentPage { get; set; }
  public int TotalPages { get; set; }
  public int PageSize { get; set; }
  public int TotalCount { get; set; }
  public IList<T> Items { get; set; }

  public PagedList(List<T> items, int count, int pageNumer, int pageSize)
  {
    TotalCount = count;
    CurrentPage = pageNumer;
    PageSize = pageSize;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    Items = items;

    //AddRange(items);
  }
  //public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
  //{
  //    var count = await source.CountAsync();

  //    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
  //    return new PagedList<T>(items, count, pageNumber, pageSize);
  //}
}

namespace KH.Dto.lookups
{
  public class LookupSearchRequestDto : PagingRequestHelper
  {
    public int? Id { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public bool? RequestFromCustomerPortal { get; set; } = false;
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public string? NameAr { get; set; }


  }
}

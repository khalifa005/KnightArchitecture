namespace KH.Dto.common
{
  public class CommonLookupFiltersRequest : PagingRequestHelper
  {
    public long? Id { get; set; }
    //public string? Search { get; set; } //for multiple use NameEn - NameAr - Description
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string? Description { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public bool? RequestFromCustomerPortal { get; set; } = false;


  }
}

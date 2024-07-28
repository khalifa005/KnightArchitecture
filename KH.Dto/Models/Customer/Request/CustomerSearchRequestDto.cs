namespace KH.Dto.Models.Customer.Request
{
  public class CustomerSearchRequestDto : PagingRequestHelper
  {
    public int? Id { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public string? IDNumber { get; set; }
    public string? Mobile { get; set; }
  }
}

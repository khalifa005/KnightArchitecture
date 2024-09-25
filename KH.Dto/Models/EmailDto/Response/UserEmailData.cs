namespace KH.Dto.Models.EmailDto.Response
{
  // UserEmailData class to hold email details
  public class UserEmailData
  {
    public string Email { get; set; }
    public string Name { get; set; }
    public string TemplateType { get; set; }

    // Order Confirmation specific data
    public string OrderId { get; set; }
    public string InvoiceLink { get; set; }
    public IEnumerable<dynamic> Products { get; set; }

    // Weekly Digest specific data
    public int TotalPurchases { get; set; }
    public double TotalSpent { get; set; }
    public IEnumerable<dynamic> RecommendedProducts { get; set; }
  }


}

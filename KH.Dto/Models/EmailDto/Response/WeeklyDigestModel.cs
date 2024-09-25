namespace KH.Dto.Models.EmailDto.Response
{
  public class WeeklyDigestModel
  {
    public string Email { get; set; }
    public string UserName { get; set; }
    public int TotalPurchases { get; set; }
    public double TotalSpent { get; set; }
    public List<ProductModel> RecommendedProducts { get; set; }
  }


}

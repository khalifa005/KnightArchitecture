namespace KH.Dto.Models.EmailDto.Response
{
  public class OrderConfirmationModel
  {
    public string UserName { get; set; }
    public string OrderId { get; set; }
    public string InvoiceLink { get; set; }
    public List<ProductModel> Products { get; set; }
  }

}

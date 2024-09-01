namespace KH.Dto.lookups.Group.Request
{
  public class GroupFilterRequest : CommonLookupFiltersRequest
  {
    //custom props used in filteration + common props
    public int? TicketCategoryId { get; set; }

  }


}

using KH.BuildingBlocks.Apis.Entities;

namespace KH.Domain.Entities.lookups;

public class Group : LookupEntity
{
  public long? TicketCategoryId { get; set; }
}

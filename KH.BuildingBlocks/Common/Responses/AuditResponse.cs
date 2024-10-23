using KH.BuildingBlocks.Apis.Entities;

namespace KH.BuildingBlocks.Apis.Responses;

public class AuditResponse
{
  public long Id { get; set; }
  public string UserId { get; set; }
  public string Type { get; set; }
  public string TableName { get; set; }
  public string DateTime { get; set; }
  public string OldValues { get; set; }
  public string NewValues { get; set; }
  public string AffectedColumns { get; set; }
  public string PrimaryKey { get; set; }

  public AuditResponse()
  {
        
  }
  public AuditResponse(Audit audit)
  {
    Id = audit.Id; // Assuming BaseEntity contains the Id field
    UserId = audit.UserId;
    Type = audit.Type;
    TableName = audit.TableName;
    DateTime = audit.DateTime.ToString("o"); // ISO 8601 format
    OldValues = audit.OldValues;
    NewValues = audit.NewValues;
    AffectedColumns = audit.AffectedColumns;
    PrimaryKey = audit.PrimaryKey;
  }
}

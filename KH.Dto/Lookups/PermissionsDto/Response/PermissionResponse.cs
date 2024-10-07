using KH.Domain.Entities;

namespace KH.Dto.Lookups.PermissionsDto.Response;

public class PermissionResponse
{
  public long Id { get; set; }
  public string NameEn { get; set; }
  public string NameAr { get; set; }
  public bool Disabled { get; set; }
  public bool Checked { get; set; }
  public List<PermissionResponse> Childrens { get; set; } = new List<PermissionResponse>();
  public int SortKey { get; set; }
  public long? ParentId { get; set; }

  public PermissionResponse()
  {


  }

  public PermissionResponse(Permission e)
  {
    Id = e.Id;
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    SortKey = e.SortKey;
    ParentId = e.ParentId;
    Childrens = e.Children.Select(x => new PermissionResponse(x)).ToList();

  }
}

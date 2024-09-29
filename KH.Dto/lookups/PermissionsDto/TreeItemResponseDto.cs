
namespace KH.Dto.lookups.PermissionsDto;

public class TreeItemResponseDto
{
  public int Id { get; set; }
  public string NameEn { get; set; }
  public string NameAr { get; set; }
  public bool Disabled { get; set; }
  public bool Checked { get; set; }
  public List<TreeItemResponseDto> Childrens { get; set; } = new List<TreeItemResponseDto>();
  public int SortKey { get; set; }
  public int? ParentID { get; set; }
}

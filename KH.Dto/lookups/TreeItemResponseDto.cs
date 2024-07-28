using KH.Dto.Models.lookups;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.lookups
{
  public class TreeItemResponseDto
  {
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public bool disabled { get; set; }
    public bool Checked { get; set; }
    public List<TreeItemResponseDto> Childrens { get; set; } = new List<TreeItemResponseDto>();
    public int SortKey { get; set; }
    public int? ParentID { get; set; }
  }

}

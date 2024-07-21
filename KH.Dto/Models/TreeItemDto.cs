using KH.Dto.Models.lookups;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.Models
{
  public class TreeItemDto
  {
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public bool disabled { get; set; }
    public bool Checked { get; set; }
    public List<TreeItemDto> Childrens { get; set; } = new List<TreeItemDto>();
    public int SortKey { get; set; }
    public int? ParentID { get; set; }
  }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Dto.Models.Email
{
  public class EscalationMatrixResponseDto
  {
    public string? DepartmentName { get; set; }
    public int? RoleID { get; set; }
    public string? RoleName { get; set; }
    public string? FullName { get; set; }

  }
}

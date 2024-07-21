using KH.Dto.Parameters.Base;

namespace KH.Dto.Parameters
{
    public class UserParameters : PagingParameters
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsDeleted { get; set; } = false;


    }
}

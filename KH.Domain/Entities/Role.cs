
namespace KH.Domain.Entities
{
    public class Role : BasicEntity
    {
        public int? ReportToRoleId { get; set; }
        public Role? ReportToRole { get; set; }

    }
}

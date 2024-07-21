
using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Entities
{
    public class UserRole : TrackerEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        [NotMapped]
        public List<RoleFunction> RoleFunctions { get; set; } = new List<RoleFunction>();

    }
}

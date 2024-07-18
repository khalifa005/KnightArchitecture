
namespace CA.Domain.Entities
{
    public class UserDepartment : BasicTrackerEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}

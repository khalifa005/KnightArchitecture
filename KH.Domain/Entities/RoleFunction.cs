
namespace CA.Domain.Entities
{
    public class RoleFunction : BasicTrackerEntity
    {
        public int SystemFunctionId { get; set; }
        public SystemFunction SystemFunction { get; set; }

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}

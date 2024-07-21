
namespace KH.Domain.Entities
{
    public class RoleFunction : TrackerEntity
    {
        public int SystemFunctionId { get; set; }
        public SystemFunction SystemFunction { get; set; }

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}

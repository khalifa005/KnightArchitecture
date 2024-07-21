
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Entities
{
    public class SystemFunction : BasicEntity
    {
		public int SortKey { get; set; }

		#region Parent
		public int? ParentID { get; set; }

		public virtual SystemFunction Parent { get; set; }
		#endregion

		public int? DependOnID { get; set; }

	}
}

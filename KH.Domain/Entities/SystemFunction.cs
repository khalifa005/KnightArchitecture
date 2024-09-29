
using KH.BuildingBlocks.Extentions.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Entities
{
  public class SystemFunction : BasicEntity
    {
		public int SortKey { get; set; }

		#region Parent
		public long? ParentID { get; set; }

		public virtual SystemFunction Parent { get; set; }
		#endregion

		public long? DependOnID { get; set; }

	}
}

using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Commons
{
    public abstract partial class TrackerEntity : BaseEntity
    {
		//what about the relations for created users !! FK's

		[Column(Order = 101)]
		public DateTime CreatedDate { get; set; }
		[Column(Order = 102)]
		public Nullable<int> CreatedById { get; set; }
        //public User? CreatedByUser { get; set; }
        [Column(Order = 103)]
		public Nullable<DateTime> UpdatedDate { get; set; }
        //public User? UpdatedByUser { get; set; }

        [Column(Order = 104)]
		public Nullable<int> UpdatedById { get; set; }
		[Column(Order = 105)]
		public bool IsDeleted { get; set; } = false;
		[Column(Order = 106)]
		public Nullable<DateTime> DeletedDate { get; set; }
		[Column(Order = 107)]
		public Nullable<int> DeletedById { get; set; }
        //public User? DeletedByUser { get; set; }

    }

}

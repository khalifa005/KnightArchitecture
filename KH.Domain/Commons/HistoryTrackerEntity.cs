using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Commons
{
    public abstract partial class HistoryTrackerEntity : BaseEntity
    {
        [Column(Order = 103)]
        public Nullable<DateTime> UpdatedDate { get; set; }
        [Column(Order = 104)]
        public Nullable<long> UpdatedById { get; set; }
    }

}

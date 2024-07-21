using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Dto.common
{
    public abstract partial class HistoryTrackerEntityDto : BaseEntityDto
    {
        [Column(Order = 103)]
        public Nullable<DateTime> UpdatedDate { get; set; }
        [Column(Order = 104)]
        public Nullable<int> UpdatedById { get; set; }
    }

}

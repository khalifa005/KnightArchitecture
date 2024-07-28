namespace KH.Dto.common
{
    public class BasicTrackerEntityDto : BaseEntityDto
    {
        public DateTime CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        //public User? CreatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedById { get; set; }
        //public User? UpdatedByUser { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public int? DeletedById { get; set; }
        //public User? DeletedByUser { get; set; }
    }

}

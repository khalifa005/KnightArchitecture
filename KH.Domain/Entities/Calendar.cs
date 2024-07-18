namespace CA.Domain.Entities
{
    public class Calendar : BasicTrackerEntity
    {
        public bool IsHoliday { get; set; }
        public DateTime HolidayDate { get; set; }
        public string? Description { get; set; }
    }
}

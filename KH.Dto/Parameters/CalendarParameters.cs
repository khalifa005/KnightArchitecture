using KH.Dto.Parameters.Base;

namespace KH.Dto.Parameters.Base
{
    public class CalendarParameters : PagingParameters
    {
        public string? Id { get; set; } 
        public bool? IsDeleted { get; set; } = false;
        public bool? FilterFutureDate { get; set; } = false;

    }


}

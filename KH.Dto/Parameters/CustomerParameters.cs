using KH.Dto.Parameters.Base;

namespace KH.Dto.Parameters.Base
{
    public class CustomerParameters : PagingParameters
    {
        public int? Id { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? IDNumber { get; set; }
        public string? Mobile { get; set; }
    }
}

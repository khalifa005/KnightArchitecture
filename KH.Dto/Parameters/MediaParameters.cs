using KH.Dto.Parameters.Base;

namespace KH.Dto.Parameters.Base
{
    public class MediaParameters : PagingParameters
    {
        public int? Id { get; set; }
        public int? ModelId { get; set; }
    }
}

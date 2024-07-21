
using KH.Dto.Parameters.Base;

namespace KH.Dto.Parameters
{
    public class SMSFollowUpParameters : PagingParameters
    {
        public int? Id { get; set; }
		public string? Status { get; set; }

		public int? ModelId { get; set; }
    }
}

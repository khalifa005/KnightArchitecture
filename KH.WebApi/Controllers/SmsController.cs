using KH.Domain.Entities;
using KH.Dto.Models.SMSDto.Form;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.WebApi.Controllers;

public class SmsController : BaseApiController
{
  public readonly ISmsService _service;
  public SmsController(ISmsService service)
  {
    _service = service;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<SmsTrackerResponse>>> Get(long id)
  {
    var res = await _service.GetSmsTrackerAsync(id);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<SmsTrackerResponse>>>> GetList(SmsTrackerFilterRequest request)
  {
    var res = await _service.GetSmsTrackerListAsync(request);
    return AsActionResult(res);
  }


  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] SmsTrackerForm request)
  {
    var res = await _service.SendSmsAsync(request);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] SmsTracker request)
  {
    var res = await _service.ResendAsync(request);
    return AsActionResult(res);
  }

}


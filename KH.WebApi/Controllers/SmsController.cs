using KH.BuildingBlocks.Apis.Extentions;
using KH.Domain.Entities;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;
using KH.Services.Sms.Contracts;

namespace KH.WebApi.Controllers;

public class SmsController : BaseApiController
{
  public readonly ISmsService _service;
  public SmsController(ISmsService service)
  {
    _service = service;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<SmsTrackerResponse>>> Get(long id, CancellationToken cancellationToken)
  {
    var res = await _service.GetSmsTrackerAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<SmsTrackerResponse>>>> GetList(SmsTrackerFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _service.GetSmsTrackerListAsync(request, cancellationToken);
    return AsActionResult(res);
  }


  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateSmsTrackerRequest request, CancellationToken cancellationToken)
  {
    var res = await _service.SendSmsAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] SmsTracker request, CancellationToken cancellationToken)
  {
    var res = await _service.ResendAsync(request, cancellationToken);
    return AsActionResult(res);
  }

}


using KH.BuildingBlocks.Apis.Extentions;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;
using KH.Services.Sms.Contracts;

namespace KH.WebApi.Controllers;

public class SmsTemplatesController : BaseApiController
{
  public readonly ISmsTemplateService _service;
  public SmsTemplatesController(ISmsTemplateService service)
  {
    _service = service;
  }

  [HttpGet("{smsType}")]
  public async Task<ActionResult<ApiResponse<SmsTemplateResponse>>> Get(string smsType, CancellationToken cancellationToken)
  {
    var res = await _service.GetSmsTemplateAsync(smsType, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<SmsTemplateResponse>>>> GetList(SmsTrackerFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _service.GetSmsTemplateListAsync(request, cancellationToken);
    return AsActionResult(res);
  }


  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateSmsTemplateRequest request, CancellationToken cancellationToken)
  {
    var res = await _service.AddSmsTemplateAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateSmsTemplateRequest request, CancellationToken cancellationToken)
  {
    var res = await _service.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await _service.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}


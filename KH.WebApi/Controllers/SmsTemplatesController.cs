using KH.BuildingBlocks.Apis;
using KH.BuildingBlocks.Apis.Responses;
using KH.Dto.Models.SMSDto.Form;
using KH.Dto.Models.SMSDto.Request;
using KH.Dto.Models.SMSDto.Response;

namespace KH.WebApi.Controllers;

public class SmsTemplatesController : BaseApiController
{
  public readonly ISmsTemplateService _service;
  public SmsTemplatesController(ISmsTemplateService service)
  {
    _service = service;
  }

  [HttpGet("{smsType}")]
  public async Task<ActionResult<ApiResponse<SmsTemplateResponse>>> Get(string smsType)
  {
    var res = await _service.GetSmsTemplateAsync(smsType);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<SmsTemplateResponse>>>> GetList(SmsTrackerFilterRequest request)
  {
    var res = await _service.GetSmsTemplateListAsync(request);
    return AsActionResult(res);
  }


  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] SmsTemplateForm request)
  {
    var res = await _service.AddSmsTemplateAsync(request);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] SmsTemplateForm request)
  {
    var res = await _service.UpdateAsync(request);
    return AsActionResult(res);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _service.DeleteAsync(id);
    return AsActionResult(res);
  }
}


using KH.Dto.Models.MediaDto.Form;
using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Response;
using KH.Dto.Models.UserDto.Form;
using KH.Dto.Models.UserDto.Request;
using KH.Dto.Models.UserDto.Response;
using KH.Helper.Extentions;
using KH.Helper.Extentions.Files;
using KH.Helper.Responses;
using KH.Services.Contracts;
using KH.Services.Features;
using Microsoft.AspNetCore.Mvc;


namespace KH.WebApi.Controllers
{
  public class MediaController : BaseApiController
  {
    private readonly IMediaService _mediaService;
    public MediaController(IMediaService mediaService)
    {
      _mediaService = mediaService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MediaResponse>>> Get(int id)
    {
      var res = await _mediaService.GetAsync(id);
      return AsActionResult(res);
    }

    [HttpPost("list")]
    public async Task<ActionResult<ApiResponse<PagedResponse<MediaResponse>>>> GetList(MediaRequest request)
    {
      var res = await _mediaService.GetListAsync(request);
      return AsActionResult(res);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> Post([FromForm] MediaForm request)
    {
      var res = await _mediaService.AddAsync(request);
      return AsActionResult(res);
    }

    [HttpPost("AddRange")]
    public async Task<ActionResult<ApiResponse<string>>> PostRange([FromForm] MediaForm request)
    {
      var res = await _mediaService.AddListAsync(request);
      return AsActionResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
      var res = await _mediaService.DeleteAsync(id);
      return AsActionResult(res);
    }

    [HttpGet("Download/{id}")]
    public async Task<IActionResult> Download(int id)
    {
      var res = await _mediaService.DownloadAsync(id);

      try
      {
        return res.Data.FileContentResult;
      }
      catch
      {
        return BadRequest();
      }

    }

  }
}

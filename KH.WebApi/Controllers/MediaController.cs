using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Apis.Responses;
using KH.BuildingBlocks.Auth.Constant;

namespace KH.WebApi.Controllers;

public class MediaController : BaseApiController
{
  private readonly IMediaService _mediaService;
  public MediaController(IMediaService mediaService)
  {
    _mediaService = mediaService;
  }
  [PermissionAuthorize(PermissionKeysConstant.Media.VIEW_MEDIA)]

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<MediaResponse>>> Get(int id)
  {
    var res = await _mediaService.GetAsync(id);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Media.LIST_MEDIA)]

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<MediaResponse>>>> GetList(MediaRequest request)
  {
    var res = await _mediaService.GetListAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Media.ADD_MEDIA)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromForm] MediaForm request)
  {
    var res = await _mediaService.AddAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Media.ADD_MEDIA_RANGE)]

  [HttpPost("AddRange")]
  public async Task<ActionResult<ApiResponse<string>>> PostRange([FromForm] MediaForm request)
  {
    var res = await _mediaService.AddListAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Media.DELETE_MEDIA)]

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _mediaService.DeleteAsync(id);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Media.DOWNLOAD_MEDIA)]

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
  [AllowAnonymous]

  [HttpPost("SubmitFormlyFormFileStream")]
  public async Task<IActionResult> SubmitFormUsingFileStream([FromForm] IFormCollection form)
  {

    var files = Request.Form.Files;

    var fileGroups = files.GroupBy(file => file.Name.Split('[')[0]); // Group files by key prefix

    foreach (var group in fileGroups)
    {
      string key = group.Key;
      foreach (var file in group)
      {
        if (file.Length > 0)
        {
          // Process each file
          var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }
        }
      }
    }

    // Handle file uploads
    foreach (var file in form.Files)
    {
      if (file.Length > 0)
      {
        // Generate a unique filename to avoid overwriting existing files
        var fileName = Path.GetFileName(file.FileName);
        var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(fileName);

        // Ensure the uploads directory exists
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(uploadsDir);

        // Combine the directory path and the unique filename
        var filePath = Path.Combine(uploadsDir, uniqueFileName);

        // Save the file to the server
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }

        System.Diagnostics.Debug.WriteLine($"File saved: {filePath}");
        return Ok(new FileResponse { FilePath = $"{filePath}", FileId = $"{filePath}" });

      }
    }


    return Ok(new FileResponse { FilePath = $"Form-submitted-successfully!" });
  }


}

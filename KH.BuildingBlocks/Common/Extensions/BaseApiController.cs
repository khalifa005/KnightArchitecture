using Microsoft.AspNetCore.Mvc;

namespace KH.BuildingBlocks.Apis.Extentions;

//[ApiVersion("1.0")]
//[Route("api/v{v:apiversion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BaseApiController : ControllerBase
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly GlobalSettings _globalSettings;

  public BaseApiController(
    IHttpContextAccessor httpContextAccessor = null,
      IOptions<GlobalSettings> globalSettings = null
    )
  {
    _httpContextAccessor = httpContextAccessor;
    _globalSettings = globalSettings != null ? globalSettings.Value : new();
  }

  public ActionResult<ApiResponse<T>> AsActionResult<T>(ApiResponse<T> response) where T : class
  {

    ObjectResult? result = new ObjectResult(response) { StatusCode = response?.StatusCode ?? (int)HttpStatusCode.BadRequest };
    return result;

  }
}

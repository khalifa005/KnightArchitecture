using KH.BuildingBlocks.Apis.Responses;
using KH.BuildingBlocks.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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


  /// <summary>
  /// This extension method returns the appropiate Action result type like OkResult, BadRequestResult, NotFoundResult, UnAuthorizedResult
  /// </summary>
  /// <typeparam name="T">this is auto detected from the type of CustomApiResponse</typeparam>
  /// <param name="response">the response object of type CustomApiResponse</param>
  /// <returns></returns>
  public ActionResult<ApiResponse<T>> AsActionResult<T>(ApiResponse<T> response) where T : class
  {
    if (response != null && response.StatusCode != (int)HttpStatusCode.OK/* && response.CorrelationId.IsNullOrWhiteSpace()*/)
    {
      //TODO: inject here?
      //response.CorrelationId = ;
    }

    ObjectResult? result = new ObjectResult(response) { StatusCode = response?.StatusCode ?? (int)HttpStatusCode.BadRequest };
    return result;

  }
}

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
  //private readonly IUserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly GlobalSettings _globalSettings;

  protected Dictionary<string, IEnumerable<string>> ErrorDetails;
  protected Dictionary<string, IEnumerable<string>> ErrorDetailsAr;
  protected ApiValidationError ValidateErrors;

  public BaseApiController(
    IHttpContextAccessor httpContextAccessor = null,
      IOptions<GlobalSettings> globalSettings = null
    )
  {
    //_userService = userService;
    _httpContextAccessor = httpContextAccessor;
    _globalSettings = globalSettings != null ? globalSettings.Value : new();

    //LoggedUser = new();
    ErrorDetails = new();
    ErrorDetailsAr = new();
    ValidateErrors = new();

    //checkValidUser();
  }

  //_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("userName", out var userName);
  //string RequestUserName() => _globalSettings.UserName;
  //_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("password", out var password);
  //string RequestUserPassword() => _globalSettings.Password;


  bool isValidLogin()
  {
    //var authHeader = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var SecretKey);

    //if (string.IsNullOrEmpty(SecretKey))
    //  return false;

    //var authenticationKey = AuthenticationHeaderValue.Parse(SecretKey);
    //if (authenticationKey.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authenticationKey.Parameter != null)
    //{
    //  byte[] key = Convert.FromBase64String(authenticationKey.Parameter);
    //  string decodeKey = Encoding.UTF8.GetString(key);
    //  string access = string.Concat(RequestUserName(), ":", RequestUserPassword());

    //  if (decodeKey == access)
    //    return true;
    //}

    return false;
  }

  void checkValidUser()
  {
    if (_httpContextAccessor != null && isValidLogin())
    {
      //LoggedUser = Task.Run(() => _userService.Login(new LoginVM { UserName = RequestUserName(), Password = RequestUserPassword() })).Result;
    }
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

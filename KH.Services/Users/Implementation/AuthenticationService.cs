
using System.Security.Claims;

namespace KH.Services.Users.Implementatoin;

public class AuthenticationService : IAuthenticationService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ITokenService _tokenService;
  private readonly ICurrentUserService _currentUserService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IHostEnvironment _env;
  private readonly ILogger<RoleService> _logger;
  public AuthenticationService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ITokenService tokenService,
    IHttpContextAccessor httpContextAccessor,
    IHostEnvironment env,
    ILogger<RoleService> logger)
  {
    _unitOfWork = unitOfWork;
    _currentUserService = currentUserService;
    _tokenService = tokenService;
    _httpContextAccessor = httpContextAccessor;
    _env = env;
    _logger = logger;
  }
  public async Task<ApiResponse<AuthenticationResponse>> RefreshUserTokenAsync(string refreshTokenValue)
  {
    var res = new ApiResponse<AuthenticationResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();
    if (string.IsNullOrEmpty(refreshTokenValue))
    {
      res.StatusCode = StatusCodes.Status400BadRequest;
      res.ErrorMessage = "empty token";
      return res;
    }

    var entityFromDB = await repository.GetByExpressionAsync(u =>
 u.RefreshToken == refreshTokenValue && u.IsDeleted == false,

 q => q.Include(u => u.UserRoles)
 .ThenInclude(ur => ur.Role)
 .ThenInclude(r => r.RolePermissions), tracking: true);


    if (entityFromDB == null)
    {
      res.StatusCode = StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid-refresh-token";
      return res;
    }

    if (entityFromDB.RefreshTokenExpiryTime < DateTime.UtcNow)
    {
      res.StatusCode = StatusCodes.Status400BadRequest;
      res.ErrorMessage = $"token-not-active.";
      return res;
    }

    //Revoke Current Refresh Token
    entityFromDB.RefreshTokenRevokedDate = DateTime.UtcNow;

    //Generate new Refresh Token and save to Database
    var refreshToken = _tokenService.GenerateRefreshToken();

    //below to activate the RefreshToken 
    entityFromDB.RefreshToken = refreshToken.Token;
    entityFromDB.RefreshTokenExpiryTime = refreshToken.Expires;
    entityFromDB.RefreshTokenCreatedDate = refreshToken.Created;
    await _unitOfWork.CommitAsync();

    var authenticationResponse = new AuthenticationResponse();
    authenticationResponse.RefreshToken = refreshToken.Token;

    //Generates new jwt
    var jwtToken = _tokenService.CreateToken(entityFromDB);
    authenticationResponse.AccessToken = jwtToken;

    res.Data = authenticationResponse;
    return res;
  }
  public async Task<ApiResponse<AuthenticationResponse>> LoginAsync(LoginRequest request)
  {
    var res = new ApiResponse<AuthenticationResponse>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<User>();


      var entityFromDB = await repository.GetByExpressionAsync(u =>
   u.Username == request.Username && u.IsDeleted == false,

   q => q.Include(u => u.UserRoles)
   .ThenInclude(ur => ur.Role)
   .ThenInclude(r => r.RolePermissions), tracking: true);



      if (entityFromDB == null || request.Password.IsNullOrEmpty() || entityFromDB.IsDeleted)
        throw new Exception("Invalid User");

      //Check Is OTP Verified at First Login

      //Hashed Password Check
      var passwordVerificationResult = new PasswordHasher<object?>()
        .VerifyHashedPassword(null, entityFromDB.Password, request.Password);
      if (passwordVerificationResult != PasswordVerificationResult.Success)
        throw new Exception("Invalid User!");

      var rolesPermissions = entityFromDB
        .UserRoles
        .SelectMany(x => x.Role.RolePermissions)
        .ToList();

      var rolesPermissionsXX = entityFromDB.UserRoles
          .SelectMany(x => x.AggregateRolePermissions(x.Role))
          .ToList();

      var jwtToken = _tokenService.CreateToken(entityFromDB);
      var authenticationResponse = new AuthenticationResponse { AccessToken = jwtToken };

      if (entityFromDB.RefreshTokenExpiryTime > DateTime.UtcNow)
      {
        authenticationResponse.RefreshToken = entityFromDB.RefreshToken;
      }
      else
      {
        var refreshToken = _tokenService.GenerateRefreshToken();

        //below to activate the RefreshToken 
        entityFromDB.RefreshToken = refreshToken.Token;
        entityFromDB.RefreshTokenExpiryTime = refreshToken.Expires;
        entityFromDB.RefreshTokenCreatedDate = refreshToken.Created;
        await _unitOfWork.CommitAsync();

        authenticationResponse.RefreshToken = refreshToken.Token;
      }

      //in case we need to set at cookies
      var response = _httpContextAccessor.HttpContext.Response;
      CookieHelper.SetCookie(response, "accessToken", authenticationResponse.AccessToken);
      CookieHelper.SetCookie(response, "refreshToken", authenticationResponse.RefreshToken);

      res.Data = authenticationResponse;

      return res;
    }
    catch (Exception ex)
    {
      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<List<Claim>> GetUserClaims(LoginRequest request)
  {
    try
    {
      var repository = _unitOfWork.Repository<User>();

      var entityFromDB = await repository.GetByExpressionAsync(u =>
   u.Username == request.Username && u.IsDeleted == false,

   q => q.Include(u => u.UserRoles)
   .ThenInclude(ur => ur.Role)
   .ThenInclude(r => r.RolePermissions)
   .Include(u => u.UserGroups)
   .Include(u => u.UserDepartments));


      if (entityFromDB == null || request.Password.IsNullOrEmpty())
        throw new Exception("Invalid User");

      //Check Is OTP Verified at First Login

      //Hashed Password Check
      var passwordVerificationResult = new PasswordHasher<object?>()
        .VerifyHashedPassword(null, entityFromDB.Password, request.Password);
      if (passwordVerificationResult != PasswordVerificationResult.Success)
        throw new Exception("Invalid User!");

      var userClaims = _tokenService.GetClaims(entityFromDB);

      return userClaims;
    }
    catch (Exception ex)
    {
      return new List<Claim>();
    }
  }

}

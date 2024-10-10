using KH.BuildingBlocks.Enums;
using KH.BuildingBlocks.Extentions;
using KH.Domain.Entities;
using KH.Dto.Models.AuthenticationDto.Response;
using KH.Dto.Models.SMSDto.Form;
using KH.PersistenceInfra.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

public class UserService : IUserService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ITokenService _tokenService;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;
  private readonly ICurrentUserService _currentUserService;
  private readonly ISmsService _smsService;
  private readonly ISmsTemplateService _smsTemplateService;
  private readonly IHttpContextAccessor _httpContextAccessor;


  public UserService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ITokenService tokenService,
    IServiceProvider serviceProvider,
    ISmsService smsService,
    ISmsTemplateService smsTemplateService,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
  {
    _unitOfWork = unitOfWork;
    _currentUserService = currentUserService;
    _tokenService = tokenService;
    _serviceProvider = serviceProvider;
    _smsService = smsService;
    _smsTemplateService = smsTemplateService;
    _mapper = mapper;
    _httpContextAccessor = httpContextAccessor;
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
        .SelectMany(x=> x.Role.RolePermissions)
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
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = new AuthenticationResponse();
      res.ErrorMessage = ex.Message;
      return res;
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
  public async Task<ApiResponse<UserDetailsResponse>> GetAsync(long id)
  {
    //below there will be multiple query technique so u can open sql profile and see translated query for each

    ApiResponse<UserDetailsResponse>? res = new ApiResponse<UserDetailsResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();

    //light user query to make sure the user exist
    var userFromDB = await repository.GetAsync(id);

    if (userFromDB == null)
      throw new Exception("Invalid Parameter");

    //complex user query using spliting
    var detailsUserFromDBX = await repository.GetAsync(id,
    q => q.Include(u => u.UserRoles)
           .ThenInclude(ur => ur.Role)
           .ThenInclude(r => r.RolePermissions)
           .ThenInclude(rp => rp.Permission)
           .Include(u => u.UserGroups)
           .ThenInclude(x => x.Group)
           .Include(u => u.UserDepartments)
           .ThenInclude(d => d.Department),
    splitQuery: true);


    //crazy query example that needs spliting or it will cause performance issues
    var detailsUserFromDB = await repository.GetAsync(id,
      q => q.Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
      .Include(u => u.UserGroups)
      .ThenInclude(x => x.Group)
      .Include(u => u.UserDepartments)
      .ThenInclude(d => d.Department));

    UserDetailsResponse userDetailsResponse = new UserDetailsResponse(detailsUserFromDB);
    //var userResponseByAutoMapper = _mapper.Map<UserDetailsResponse>(detailsUserFromDB);

    res.Data = userDetailsResponse;
    return res;
  }
  public async Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request)
  {
    ApiResponse<UserDetailsResponse>? res = new ApiResponse<UserDetailsResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();

    //use this if u need to get all users related to this filter
    var result = await repository.FindByAsync(u =>
    u.FirstName.Contains(request.Search)
    || u.LastName.Contains(request.Search),

    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));

    //use this if u need to get one user related to this filter
    var entityFromDB = await repository.GetByExpressionAsync(u =>
    u.FirstName.Contains(request.Search)
    || u.LastName.Contains(request.Search),

    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));

    if (entityFromDB == null)
    {
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.ErrorMessage = "invalid user";
    }

    UserDetailsResponse entityResponse = new UserDetailsResponse(entityFromDB);

    res.Data = entityResponse;
    return res;
  }
  public async Task<ApiResponse<PagedResponse<UserListResponse>>> GetListAsync(UserFilterRequest request)
  {
    var repository = _unitOfWork.Repository<User>();

    //example with internal predicate
    var result = await repository.GetPagedAsync(
      request.PageIndex,
      request.PageSize,
      u =>
    u.FirstName.Contains(request.Search)
    || u.LastName.Contains(request.Search),

    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));


    var userListResponses = result.Select(x => new UserListResponse(x)).ToList();

    var pagedResponse = new PagedResponse<UserListResponse>(
      userListResponses,
       result.CurrentPage,
       result.TotalPages,
       result.PageSize,
       result.TotalCount);

    ApiResponse<PagedResponse<UserListResponse>> apiResponse = new ApiResponse<PagedResponse<UserListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingIQueryableAsync(UserFilterRequest request)
  {
    var repository = _unitOfWork.Repository<User>();
    IQueryable<User> query = repository.GetQueryable();


    if (!string.IsNullOrEmpty(request.Email))
    {
      query = query.Where(u => u.Email.Contains(request.Email));
    }

    if (!string.IsNullOrEmpty(request.UserName))
    {
      query = query.Where(u => u.Username.Contains(request.UserName));
    }

    if (!string.IsNullOrEmpty(request.Search))
    {
      query = query.Where(u =>
      u.MobileNumber.Contains(request.Search)
      || u.FirstName.Contains(request.Search));
    }

    query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                 .Include(u => u.UserGroups)
                 .Include(u => u.UserDepartments);

    var usersFromDB = await repository.GetPagedUsingQueryAsync(request.PageIndex, request.PageSize, query);

    var userListResponses = usersFromDB.Select(x => new UserListResponse(x)).ToList();

    var pagedResponse = new PagedResponse<UserListResponse>(
      userListResponses,
       usersFromDB.CurrentPage,
       usersFromDB.TotalPages,
       usersFromDB.PageSize,
       usersFromDB.TotalCount);

    ApiResponse<PagedResponse<UserListResponse>> apiResponse = new ApiResponse<PagedResponse<UserListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request)
  {
    var repository = _unitOfWork.Repository<User>();

    //QuerySplittingBehavior Warning
    var pagedUsers = await repository.GetPagedWithProjectionAsync<UserListResponse>(
    pageNumber: 1,
    pageSize: 10,
    filterExpression: u => u.IsDeleted == request.IsDeleted, // Filter by 
    projectionExpression: u => new UserListResponse
    {
      Id = u.Id,
      Username = u.Username,
      LastName = u.LastName,
      UserRoles = u.UserRoles.Select(ur => new UserRoleResponse(ur)).ToList(),
      DepartmentNames = u.UserDepartments.Select(ud => ud.Department.NameEn).ToList()
    },
    include: query => query
        .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
        .Include(u => u.UserDepartments).ThenInclude(ud => ud.Department),  // Include related data
    orderBy: query => query.OrderBy(u => u.Id),  // Sort by Id
    tracking: false  // Disable tracking for read-only queries
);


    var userListResponses = pagedUsers.Select(x => x).ToList();

    var pagedResponse = new PagedResponse<UserListResponse>(
      userListResponses,
       pagedUsers.CurrentPage,
       pagedUsers.TotalPages,
       pagedUsers.PageSize,
       pagedUsers.TotalCount);

    ApiResponse<PagedResponse<UserListResponse>> apiResponse = new ApiResponse<PagedResponse<UserListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedResponse;

    return apiResponse;
  }
  public async Task<ApiResponse<string>> AddAsync(UserForm request)
  {
    //db context will handel saving it auto
    var actionMadeByUserId = _serviceProvider.GetUserId();
    var currentUserId = _currentUserService.UserId;

    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync();

    //no nned for try catch we have global exception handler test it
    try
    {
      if (request.Id.HasValue)
        throw new Exception("invalid-user-id-in-add-mode");

      //-- Check User Duplication
      var isThereDuplicatedUser = await IsThereMatchedUser(request.Email, request.Username);
      if (isThereDuplicatedUser)
        throw new Exception("duplicated username or email");

      var isThereDuplicatedUserWithTheSamePhone = await IsThereMatchedUserWithTheSamePhoneNumber(request.MobileNumber);
      if (isThereDuplicatedUserWithTheSamePhone)
        throw new Exception("duplicated phone number");

      //custom mapping
      var userEntity = request.ToEntity();
      //auto mapper
      var userEntityByAutoMapper = _mapper.Map<User>(request);

      //check what happen to craetion date for user roles and user groups +
      //move mapping to the automapper or custom entity
      userEntity.UserRoles = request.RoleIds.Select(roleId => new UserRole
      {
        RoleId = roleId
      }).ToList();

      //this will ensure that wilsave all related user in one query
      userEntity.UserDepartments = new List<UserDepartment>() {
        new UserDepartment { DepartmentId = request.DepartmentId.Value}
      };

      userEntity.UserGroups = new List<UserGroup>() {
        new UserGroup { GroupId = request.GroupId!.Value}
      };


      var repository = _unitOfWork.Repository<User>();

      await repository.AddAsync(userEntity);

      await _unitOfWork.CommitAsync();

      var smsWelcomeTemplateResult = await _smsTemplateService.GetSmsTemplateAsync(SmsTypeEnum.WelcomeUser.ToString());
      if (smsWelcomeTemplateResult.StatusCode == StatusCodes.Status200OK && smsWelcomeTemplateResult.Data is object)
      {
        var template = smsWelcomeTemplateResult.Data;
        var templateContetBasedOnLanguage = _smsTemplateService.GetTemplateForLanguage(template, LanguageEnum.English);
        var welcomeSmsMessageFormatted = _smsTemplateService.ReplaceWelcomeSmsPlaceholders(templateContetBasedOnLanguage, userEntity);

        var smsTrackerForm = new SmsTrackerForm()
        {
          MobileNumber = userEntity.MobileNumber,
          Message = welcomeSmsMessageFormatted,
          Model = ModelEnum.User.ToString(),
          ModelId = userEntity.Id
        };

        var smsResult = await _smsService.SendSmsAsync(smsTrackerForm);
      }

      await _unitOfWork.CommitTransactionAsync();

      res.Data = userEntity.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync();

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> AddListAsync(List<UserForm> request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
    await _unitOfWork.BeginTransactionAsync();

    try
    {
      //custom mapping
      var userEntities = request.Select(x => x.ToEntity()).ToList();

      var repository = _unitOfWork.Repository<User>();

      await repository.AddRangeAsync(userEntities);
      await _unitOfWork.CommitAsync();


      await _unitOfWork.CommitTransactionAsync();

      res.Data = String.Join(",", userEntities.Select(x => x.Id));
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync();

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> UpdateAsync(UserForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    //mapper
    //var userEntityByAutoMapper = _mapper.Map<User>(request);
    var userEntityByAutoMapper = request.ToEntity();

    var userRoleRepository = _unitOfWork.Repository<UserRole>();
    var repository = _unitOfWork.Repository<User>();

    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var userFromDb = await repository.GetAsync(request.Id.Value, include: x => x.Include(x => x.UserRoles), tracking: true);

      if (userFromDb == null)
        throw new Exception("Invalid Parameter");

      //Add the new props here ..etc
      userFromDb.MobileNumber = userEntityByAutoMapper.MobileNumber;
      userFromDb.LastName = userEntityByAutoMapper.LastName;
      userFromDb.FirstName = userEntityByAutoMapper.FirstName;
      userFromDb.MiddleName = userEntityByAutoMapper.MiddleName;
      userFromDb.BirthDate = userEntityByAutoMapper.BirthDate;

      if (userEntityByAutoMapper?.UserRoles?.Count > 0)
      {
        // Ensure userFromDb and request are not null
        if (userFromDb?.UserRoles != null && request?.RoleIds != null)
        {
          // Get existing roles from the database that match the RoleIds in the request
          var existingUserRoles = userFromDb.UserRoles
              .Where(userRole => request.RoleIds.Contains(userRole.RoleId))
              .ToList();

          // Identify new roles from AutoMapper that are not already in the database or in the matched roles
          var newUserRoles = userEntityByAutoMapper.UserRoles
              .Where(mappedRole => !userFromDb.UserRoles.Any(dbRole => dbRole.RoleId == mappedRole.RoleId)
                                 && !existingUserRoles.Any(matchedRole => matchedRole.RoleId == mappedRole.RoleId))
              .ToList();

          // Combine the existing and new roles
          var updatedUserRoles = existingUserRoles.Concat(newUserRoles).ToList();

          // Update the UserRoles collection with the combined list of roles; EF Core will track and apply changes
          userFromDb.UserRoles = updatedUserRoles;
        }
      }


      await _unitOfWork.CommitAsync();

      await _unitOfWork.CommitTransactionAsync();

      res.Data = userEntityByAutoMapper.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync();

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> UpdateRangeUsingBatchAsync(UserForm request)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
    var repository = _unitOfWork.Repository<User>();

    await repository.BatchUpdateAsync(
        x => x.IsDeleted == true,
        x => x.SetProperty(y => y.IsOtpVerified, false)
    );

    await repository.BatchDeleteAsync(x => x.DeletedDate < DateTime.UtcNow.AddMonths(-6));

    return res;

  }
  public async Task<ApiResponse<string>> DeleteAsync(long id)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<User>();

      var userFromDB = await repository.GetAsync(id);
      if (userFromDB == null)
        throw new Exception("Invalid user");

      repository.Delete(userFromDB);
      await _unitOfWork.CommitAsync();

      res.Data = userFromDB.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {

      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.Data = ex.Message;
      res.ErrorMessage = ex.Message;
      return res;
    }
  }
  public async Task<ApiResponse<string>> ResetDepartmentsAsync(long id)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();
    var user = await repository.GetAsync(
      id,
      q => q.Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .Include(u => u.UserGroups)
      .Include(u => u.UserDepartments), tracking: true);

    if (user == null)
    {
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.ErrorMessage = "user-not-found";
      return res;
    }
    //example on hard delete of user departments
    //we also can keep it and just mart it as IsDeleted = true
    user.UserDepartments.Clear();

    await _unitOfWork.CommitAsync();

    //another approach is using soft delete

    // Check if there are departments to delete
    if (user.UserDepartments != null && user.UserDepartments.Any())
    {
      var userDepartmentRepository = _unitOfWork.Repository<UserDepartment>();

      // Delete each UserDepartment record associated with the user
      foreach (var userDepartment in user.UserDepartments.ToList())
      {
        userDepartmentRepository.Delete(userDepartment);
      }

      // Save changes
      await _unitOfWork.CommitAsync();
    }

    return res;
  }
  private async Task<bool> IsThereMatchedUser(string email, string username)
  {
    var repository = _unitOfWork.Repository<User>();

    var result = await repository.FindByAsync(u =>
    u.Username == username ||
    u.Email == email);

    if (result.Any())
      return true;

    return false;
  }
  private async Task<bool> IsThereMatchedUserWithTheSamePhoneNumber(string phoneNumber)
  {
    var repository = _unitOfWork.Repository<User>();

    var result = await repository.FindByAsync(u =>
    u.MobileNumber == phoneNumber);

    if (result.Any())
      return true;

    return false;
  }
  private async Task<IEnumerable<User>> FindUsersWithIncludesAsync(Expression<Func<User, bool>> predicate)
  {
    //FindUsersWithIncludesAsync(u => u.FirstName.Contains(search) || u.LastName.Contains(search));
    var repository = _unitOfWork.Repository<User>();

    //example with internal predicate
    var result = await repository.FindByAsync(u =>
    u.FirstName.Contains("search")
    || u.LastName.Contains("search2"),

    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));

    return await repository.FindByAsync(predicate, q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                                        .Include(u => u.UserGroups)
                                                        .Include(u => u.UserDepartments));
  }
  private async Task<int> CountUsersByAsync(Expression<Func<User, bool>> predicate)
  {
    var repository = _unitOfWork.Repository<User>();
    return await repository.CountByAsync(predicate);
  }

}


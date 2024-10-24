using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.SMSDto.Request;
using KH.Services.Emails.Contracts;
using KH.Services.Sms.Contracts;

namespace KH.Services.Users.Implementation;

public class UserManagementService : IUserManagementService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ITokenService _tokenService;
  private readonly IMapper _mapper;
  private readonly ICurrentUserService _currentUserService;
  private readonly ISmsService _smsService;
  private readonly ISmsTemplateService _smsTemplateService;
  private readonly IEmailService _emailService;

  private readonly IUserNotificationService _userNotificationService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IUserValidationService _userValidationService;
  private readonly IHostEnvironment _env;
  private readonly ILogger<UserManagementService> _logger;
  public UserManagementService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ITokenService tokenService,
    ISmsService smsService,
    ISmsTemplateService smsTemplateService,
    IEmailService emailService,
    IMapper mapper,
    IUserValidationService userValidationService,
    IUserNotificationService userNotificationService,
    IHttpContextAccessor httpContextAccessor,
    IHostEnvironment env,
    ILogger<UserManagementService> logger)
  {
    _unitOfWork = unitOfWork;
    _currentUserService = currentUserService;
    _tokenService = tokenService;
    _userValidationService = userValidationService;
    _smsService = smsService;
    _smsTemplateService = smsTemplateService;
    _emailService = emailService;
    _mapper = mapper;
    _httpContextAccessor = httpContextAccessor;
    _userNotificationService = userNotificationService;
    _env = env;
    _logger = logger;
  }

  public async Task<ApiResponse<string>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken)
  {
    var currentUserId = _currentUserService.UserId;

    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {
      if (request.Id.HasValue)
        throw new Exception("invalid-user-id-in-add-mode");

      //-- Check User Duplication
      var isThereDuplicatedUser = await _userValidationService.IsThereMatchedUserAsync(request.Email!, request.Username!, cancellationToken: cancellationToken);
      if (isThereDuplicatedUser)
        throw new Exception("duplicated username or email");

      var isThereDuplicatedUserWithTheSamePhone = await _userValidationService.IsThereMatchedUserWithTheSamePhoneNumberAsync(request.MobileNumber!, cancellationToken: cancellationToken);
      if (isThereDuplicatedUserWithTheSamePhone)
        throw new Exception("duplicated phone number");

      //custom mapping
      var userEntity = request.ToEntity();
      //auto mapper
      var userEntityByAutoMapper = _mapper.Map<User>(request);

      userEntity.UserRoles = request.RoleIds.Select(roleId => new UserRole
      {
        RoleId = roleId
      }).ToList();

      userEntity.UserDepartments = new List<UserDepartment>() {
        new UserDepartment { DepartmentId = request.DepartmentId.Value}
      };

      userEntity.UserGroups = new List<UserGroup>() {
        new UserGroup { GroupId = request.GroupId!.Value}
      };

      var repository = _unitOfWork.Repository<User>();

      await repository.AddAsync(userEntity);

      await _unitOfWork.CommitAsync();

      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

      // Now, handle SMS and Email after the transaction is successful
      var smsResult = await _userNotificationService.SendUserWelcomeSmsAsync(userEntity, cancellationToken);
      if (smsResult.StatusCode != StatusCodes.Status200OK)
      {
        _logger.LogError($"Failed to send SMS to user {userEntity.Id}: {smsResult.ErrorMessage}");
        // Handle the failure but don't rollback the transaction
      }

      var emailResult = await _userNotificationService.SendUserWelcomeEmailAsync(userEntity, cancellationToken);
      if (emailResult.StatusCode != StatusCodes.Status200OK)
      {
        _logger.LogError($"Failed to send email to user {userEntity.Id}: {emailResult.ErrorMessage}");
        // Handle the failure but don't rollback the transaction
      }
      res.Data = userEntity.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<ApiResponse<string>> AddListAsync(List<CreateUserRequest> request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {
      var userEntities = request.Select(x => x.ToEntity()).ToList();

      var repository = _unitOfWork.Repository<User>();

      await repository.AddRangeAsync(userEntities, cancellationToken: cancellationToken);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);


      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

      res.Data = String.Join(",", userEntities.Select(x => x.Id));
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<ApiResponse<string>> UpdateAsync(CreateUserRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var userEntityByAutoMapper = request.ToEntity();

    var userRoleRepository = _unitOfWork.Repository<UserRole>();
    var repository = _unitOfWork.Repository<User>();

    await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

    try
    {
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var userFromDb = await repository
        .GetAsync(request.Id.Value,
        include: x => x.Include(x => x.UserRoles),
        tracking: true,
        cancellationToken: cancellationToken);

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
          //ef will delete the missing roles that was being tracked then won't exist anymore
          userFromDb.UserRoles = updatedUserRoles;
        }
      }


      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

      await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

      res.Data = userEntityByAutoMapper.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollBackTransactionAsync(cancellationToken: cancellationToken);

      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<ApiResponse<string>> UpdateRangeUsingBatchAsync(CreateUserRequest request, CancellationToken cancellationToken)
  {
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
    var repository = _unitOfWork.Repository<User>();

    await repository.BatchUpdateAsync(
        x => x.IsDeleted == true,
        x => x.SetProperty(y => y.IsOtpVerified, false)
    );

    await repository.BatchDeleteAsync(x => x.DeletedDate < DateTime.UtcNow.AddMonths(-6), cancellationToken: cancellationToken);

    return res;

  }
  public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    try
    {
      var repository = _unitOfWork.Repository<User>();

      var userFromDB = await repository.GetAsync(id, cancellationToken: cancellationToken);
      if (userFromDB == null)
        throw new Exception("Invalid user");

      repository.Delete(userFromDB);
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

      res.Data = userFromDB.Id.ToString();
      return res;
    }
    catch (Exception ex)
    {
      return ex.HandleException(res, _env, _logger);
    }
  }
  public async Task<ApiResponse<string>> ResetDepartmentsAsync(long id, CancellationToken cancellationToken)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();
    var user = await repository.GetAsync(
      id,
      q => q.Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .Include(u => u.UserGroups)
      .Include(u => u.UserDepartments),
      tracking: true,
      cancellationToken: cancellationToken);

    if (user == null)
    {
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.ErrorMessage = "user-not-found";
      return res;
    }
    //example on hard delete of user departments
    //we also can keep it and just mart it as IsDeleted = true
    user.UserDepartments.Clear();

    await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

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
      await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
    }

    return res;
  }
  
}

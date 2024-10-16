namespace KH.Services.Users.Implementation;

public class UserValidationService : IUserValidationService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ITokenService _tokenService;
  private readonly ICurrentUserService _currentUserService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IHostEnvironment _env;
  private readonly ILogger<RoleService> _logger;
  public UserValidationService(
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

  public async Task<bool> IsThereMatchedUserAsync(string email, string username, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<User>();

    var result = await repository.FindByAsync(u =>
    u.Username == username ||
    u.Email == email);

    if (result.Any())
      return true;

    return false;
  }
  public async Task<bool> IsThereMatchedUserWithTheSamePhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<User>();

    var result = await repository.FindByAsync(u =>
    u.MobileNumber == phoneNumber);

    if (result.Any())
      return true;

    return false;
  }

}

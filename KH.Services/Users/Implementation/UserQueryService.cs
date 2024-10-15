namespace KH.Services.Users.Implementation;

public class UserQueryService : IUserQueryService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ITokenService _tokenService;
  private readonly ICurrentUserService _currentUserService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IHostEnvironment _env;
  private readonly ILogger<RoleService> _logger;
  public UserQueryService(
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

  public Task<ApiResponse<string>> CountUsersByAsync(UserFilterRequest request)
  {
    throw new NotImplementedException();
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
}

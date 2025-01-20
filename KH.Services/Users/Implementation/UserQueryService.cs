using KH.BuildingBlocks.Auth.Contracts;
using KH.Dto.Lookups.PermissionsDto.Response;
using KH.Services.Auth.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

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

  public Task<ApiResponse<string>> CountUsersByAsync(UserFilterRequest request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
  public async Task<ApiResponse<UserDetailsResponse>> GetAsync(long id, CancellationToken cancellationToken)
  {
    //below there will be multiple query technique so u can open sql profile and see translated query for each

    ApiResponse<UserDetailsResponse>? res = new ApiResponse<UserDetailsResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();

    //light user query to make sure the user exist
    var userFromDB = await repository.GetAsync(id);
    
    if (userFromDB == null)
    {
      res.StatusCode = (int)HttpStatusCode.BadRequest;
      res.ErrorMessage = "invalid user";

      return res;
    }

    //complex user query using splitting
    var detailsUserFromDB = await repository.GetAsync(id,
    q => q.Include(u => u.UserRoles)
           .ThenInclude(ur => ur.Role)
           .ThenInclude(r => r.RolePermissions)
           .ThenInclude(rp => rp.Permission)
           .Include(u => u.UserGroups)
           .ThenInclude(x => x.Group)
           .Include(u => u.UserDepartments)
           .ThenInclude(d => d.Department),
    splitQuery: true);

    if (detailsUserFromDB is null)
    {
      res.StatusCode = StatusCodes.Status400BadRequest;
      res.ErrorMessage = "invalid user";
      return res;
    }

    if (false)
    {
      //crazy query example that needs splitting or it will cause performance issues
      var detailsUserFromDBx = await repository.GetAsync(id,
      q => q.Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .ThenInclude(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
      .Include(u => u.UserGroups)
      .ThenInclude(x => x.Group)
      .Include(u => u.UserDepartments)
      .ThenInclude(d => d.Department));

    }

    UserDetailsResponse userDetailsResponse = new UserDetailsResponse(detailsUserFromDB);

    res.Data = userDetailsResponse;
    return res;
  }
  public async Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request, CancellationToken cancellationToken)
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
      return res;
    }

    UserDetailsResponse entityResponse = new UserDetailsResponse(entityFromDB);

    res.Data = entityResponse;
    return res;
  }
  public async Task<ApiResponse<PagedList<UserListResponse>>> GetListUsingIQueryableAsync(UserFilterRequest request, CancellationToken cancellationToken)
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

    var userListResponses = usersFromDB.Items.Select(x => new UserListResponse(x)).ToList();
    ApiResponse<PagedList<UserListResponse>> apiResponse = new ApiResponse<PagedList<UserListResponse>>((int)HttpStatusCode.OK);

    apiResponse.Data = new PagedList<UserListResponse>(
      userListResponses,
      count: usersFromDB.TotalCount,
      pageNumer: usersFromDB.CurrentPage,
      pageSize: usersFromDB.PageSize);

    return apiResponse;
  }
  public async Task<ApiResponse<PagedList<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request, CancellationToken cancellationToken)
  {
    var repository = _unitOfWork.Repository<User>();

    var pagedUsers = await repository.GetPagedWithProjectionAsync<UserListResponse>(
    pageNumber: request.PageIndex,
    pageSize: request.PageSize,
    filterExpression: u => u.IsDeleted == request.IsDeleted
     && (string.IsNullOrEmpty(request.UserName) || u.Username.Contains(request.UserName))
     && (string.IsNullOrEmpty(request.Email) || u.Email.Contains(request.Email))
    && (!request.Id.HasValue || u.Id == request.Id), // Filter by 
    projectionExpression: u => new UserListResponse
    {
      Id = u.Id,
      Username = u.Username,
      FirstName = u.FirstName,
      LastName = u.LastName,
      IsDeleted = u.IsDeleted,
      CreatedDate = u.CreatedDate,
      UserRoles = u.UserRoles.Select(ur => new UserRoleResponse(ur)).ToList(),
      DepartmentNames = u.UserDepartments.Select(ud => ud.Department!.NameEn).ToList()
    },
    include: query => query
        .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
        .Include(u => u.UserDepartments).ThenInclude(ud => ud.Department),
    orderBy: query => query.OrderBy(u => u.Id),  
    tracking: false  
);


    ApiResponse<PagedList<UserListResponse>> apiResponse = new ApiResponse<PagedList<UserListResponse>>((int)HttpStatusCode.OK);
    apiResponse.Data = pagedUsers;

    return apiResponse;
  }


  public async Task<ApiResponse<List<PermissionResponse>>> GetUserPermissionsListAsync(CancellationToken cancellationToken)
  {
    var userRoles = _currentUserService.RolesIds;

    var repository = _unitOfWork.Repository<Permission>();


    // Check if the user has the Super Admin role
    var hasSuperAdminRole = RoleChecker.HasSuperAdminRole(userRoles);

    // Build the filter expression
    Expression<Func<Permission, bool>> filter;
    if (hasSuperAdminRole)
    {
      // Super Admin: Get all active permissions
      filter = x => x.IsDeleted == false;
    }
    else
    {
      // Regular user: Filter based on their roles
      filter = x =>
          x.IsDeleted == false &&
          x.RolePermissions.Any(rp => userRoles.Contains(rp.RoleId.ToString()));
    }

    // Apply the filter directly in the repository call


    // Apply the filter directly in the repository call
    var listResult = await repository.GetAllAsync(
        filter: filter, // Filter directly in the repository
        include: null,
        useCache: false,
        cancellationToken: cancellationToken);

    // Map the results
    var mappedListResult = listResult
        .Select(x => new PermissionResponse(x))
        .ToList();

    // Prepare API response
    var apiResponse = new ApiResponse<List<PermissionResponse>>((int)HttpStatusCode.OK)
    {
      Data = mappedListResult
    };

    return apiResponse;
  }


}

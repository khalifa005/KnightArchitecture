using AutoMapper;
using Azure.Core;
using KH.Domain.Entities;
using KH.Dto.Models.UserDto.Response;
using KH.Helper.Extentions.Methods;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

public class UserService : IUserService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IServiceProvider _serviceProvider;
  private readonly IMapper _mapper;

  public UserService(
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider,
    IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _serviceProvider = serviceProvider;
    _mapper = mapper;
  }

  public async Task<ApiResponse<UserDetailsResponse>> GetAsync(long id)
  {
    //define our api res 
    ApiResponse<UserDetailsResponse>? res = new ApiResponse<UserDetailsResponse>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();

    //light user query to make sure the user exist
    var userFromDB = await repository.GetAsync(id);

    if (userFromDB == null)
      throw new Exception("Invalid Parameter");

    var detailsUserFromDB = await repository.GetAsync(id,
      q => q.Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .Include(u => u.UserGroups)
      .ThenInclude(x => x.Group)
      .Include(u => u.UserDepartments)
      .ThenInclude(d => d.Department));

    UserDetailsResponse userDetailsResponse = new UserDetailsResponse(detailsUserFromDB);
    //var userResponseByAutoMapper = _mapper.Map<UserDetailsResponse>(detailsUserFromDB);

    res.Data = userDetailsResponse;
    return res;
  }
  /// <summary>
  /// translated query will be
  /// // Define the SQL query string
  //string sqlQuery = @"
  //  SELECT 
  //      [u].[Id], 
  //      [u].[BirthDate], 
  //      [u].[CreatedById], 
  //      [u].[CreatedDate], 
  //      [u].[DeletedById], 
  //      [u].[DeletedDate], 
  //      [u].[Email], 
  //      [u].[FirstName], 
  //      [u].[IsDeleted], 
  //      [u].[LastAssignDateAsAssignTo], 
  //      [u].[LastAssignDateAsCaseOwner], 
  //      [u].[LastAssignDateAsSupervisor], 
  //      [u].[LastName], 
  //      [u].[MiddleName], 
  //      [u].[MobileNumber], 
  //      [u].[UpdatedById], 
  //      [u].[UpdatedDate], 
  //      [u].[Username], 
  //      [t].[Id], 
  //      [t].[CreatedById], 
  //      [t].[CreatedDate], 
  //      [t].[DeletedById], 
  //      [t].[DeletedDate], 
  //      [t].[IsDeleted], 
  //      [t].[RoleId], 
  //      [t].[UpdatedById], 
  //      [t].[UpdatedDate], 
  //      [t].[UserId], 
  //      [t].[Id0], 
  //      [t].[CreatedById0], 
  //      [t].[CreatedDate0], 
  //      [t].[DeletedById0], 
  //      [t].[DeletedDate0], 
  //      [t].[Description], 
  //      [t].[IsDeleted0], 
  //      [t].[NameAr], 
  //      [t].[NameEn], 
  //      [t].[ReportToRoleId], 
  //      [t].[UpdatedById0], 
  //      [t].[UpdatedDate0], 
  //      [u1].[Id], 
  //      [u1].[CreatedById], 
  //      [u1].[CreatedDate], 
  //      [u1].[DeletedById], 
  //      [u1].[DeletedDate], 
  //      [u1].[GroupId], 
  //      [u1].[IsDeleted], 
  //      [u1].[UpdatedById], 
  //      [u1].[UpdatedDate], 
  //      [u1].[UserId], 
  //      [u2].[Id], 
  //      [u2].[CreatedById], 
  //      [u2].[CreatedDate], 
  //      [u2].[DeletedById], 
  //      [u2].[DeletedDate], 
  //      [u2].[DepartmentId], 
  //      [u2].[IsDeleted], 
  //      [u2].[UpdatedById], 
  //      [u2].[UpdatedDate], 
  //      [u2].[UserId]
  //  FROM 
  //      [Users] AS [u]
  //  LEFT JOIN (
  //      SELECT 
  //          [u0].[Id], 
  //          [u0].[CreatedById], 
  //          [u0].[CreatedDate], 
  //          [u0].[DeletedById], 
  //          [u0].[DeletedDate], 
  //          [u0].[IsDeleted], 
  //          [u0].[RoleId], 
  //          [u0].[UpdatedById], 
  //          [u0].[UpdatedDate], 
  //          [u0].[UserId], 
  //          [r].[Id] AS [Id0], 
  //          [r].[CreatedById] AS [CreatedById0], 
  //          [r].[CreatedDate] AS [CreatedDate0], 
  //          [r].[DeletedById] AS [DeletedById0], 
  //          [r].[DeletedDate] AS [DeletedDate0], 
  //          [r].[Description], 
  //          [r].[IsDeleted] AS [IsDeleted0], 
  //          [r].[NameAr], 
  //          [r].[NameEn], 
  //          [r].[ReportToRoleId], 
  //          [r].[UpdatedById] AS [UpdatedById0], 
  //          [r].[UpdatedDate] AS [UpdatedDate0]
  //      FROM 


  ///// </summary>
  ///// <param name="request"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request)
  {
    var repository = _unitOfWork.Repository<User>();

    var result = await repository.FindByAsync(u =>
    u.FirstName.Contains(request.Search)
    || u.LastName.Contains(request.Search),

    q => q.Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
    .Include(u => u.UserDepartments));

    throw new NotImplementedException();
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
  /// <summary>
  /// converted sql 
  //  string sqlQuery = @"
  //    SELECT 
  //        [u].[Id], 
  //        [u].[Username]
  //    FROM 
  //        [Users] AS [u]
  //    WHERE 
  //        [u].[IsDeleted] = CAST(0 AS bit) -- Filter to include only non-deleted users
  //    ORDER BY 
  //        (SELECT 1) -- A dummy ORDER BY clause; can be replaced with actual column(s) for ordering
  //    OFFSET @__p_0 ROWS -- Skip the number of rows specified by @__p_0
  //    FETCH NEXT @__p_1 ROWS ONLY -- Fetch the number of rows specified by @__p_1
  //";

  //  // Define the parameters for the query
  //  int offset = 0; // Number of rows to skip
  //  int fetchNext = 10; // Number of rows to fetch

  //  // Create the command object with the query and parameters
  //  SqlCommand command = new SqlCommand(sqlQuery);
  //  command.Parameters.AddWithValue("@__p_0", offset);
  //command.Parameters.AddWithValue("@__p_1", fetchNext);

  // Execute the query (this step is context-dependent, e.g., using a connection object)

  /// </summary>
  /// <param name="request"></param>
  /// <returns></returns>
  public async Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request)
  {
    var repository = _unitOfWork.Repository<User>();

    var pagedUsers = await repository.GetPagedWithProjectionAsync<UserListResponse>(
    pageNumber: 1,
    pageSize: 10,
    filterExpression: u => u.IsDeleted, // Filter by 
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

    //define our api res 
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    await _unitOfWork.BeginTransactionAsync();

    //no nned for try catch we have global exception handler test it
    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      //all validation should be in fluent validation side
      if (request.Email.IsNullOrEmpty())
        throw new Exception("email is required");

      if (request.RoleIds == null || request.RoleIds.Length <= 0)
        throw new Exception("you must select at least one role");

      if (!request.DepartmentId.HasValue)
        throw new Exception("you must select department");

      //-- Check User Duplication
      //await this.CheckDuplictedUser(userObj);

      //custom mapping
      var userEntity = request.ToEntity();
      //auto mapper
      var userEntityByAutoMapper = _mapper.Map<User>(request);

      //check what happen to craetion date for user roles and user groups +
      //move mapping to the automapper or custom entity
      userEntityByAutoMapper.UserRoles = request.RoleIds.Select(roleId => new UserRole
      {
        RoleId = roleId
      }).ToList();

      //this will ensure that wilsave all related user in one query
      userEntityByAutoMapper.UserDepartments = new List<UserDepartment>() {
        new UserDepartment { DepartmentId = request.DepartmentId.Value}
      };

      userEntityByAutoMapper.UserGroups = new List<UserGroup>() {
        new UserGroup { GroupId = request.GroupId!.Value}
      };


      var repository = _unitOfWork.Repository<User>();

      await repository.AddAsync(userEntityByAutoMapper);
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
  public async Task<ApiResponse<string>> AddListAsync(List<UserForm> request)
  {
    //db context will handel saving it auto
    var actionMadeByUserId = _serviceProvider.GetUserId();

    //define our api res 
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);
    await _unitOfWork.BeginTransactionAsync();

    try
    {
      if (request == null)
        throw new Exception("Invalid Parameter");

      //-- Check User Duplication for each by national id and email
      //await this.CheckDuplictedUser(userObj);

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
    //define our api res 
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    //auto mapper
    var userEntityByAutoMapper = _mapper.Map<User>(request);

    var repository = _unitOfWork.Repository<User>();
    await _unitOfWork.BeginTransactionAsync();

    try
    {
      //there will be fluend validation rules
      if (request == null)
        throw new Exception("Invalid Parameter");

      //all validation should be in fluent validation side
      if (!request.Id.HasValue)
        throw new Exception("id is required");

      var userFromDb = await repository.GetAsync(request.Id.Value);

      if (userFromDb == null)
        throw new Exception("Invalid Parameter");

      //Add the new props here ..etc
      userFromDb.MobileNumber = userEntityByAutoMapper.MobileNumber;
      userFromDb.Email = userEntityByAutoMapper.Email;
      userFromDb.Username = userEntityByAutoMapper.Username;
      userFromDb.LastName = userEntityByAutoMapper.LastName;
      userFromDb.FirstName = userEntityByAutoMapper.FirstName;
      userFromDb.MiddleName = userEntityByAutoMapper.MiddleName;
      userFromDb.BirthDate = userEntityByAutoMapper.BirthDate;



      repository.Update(userFromDb);
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
  public Task<ApiResponse<AuthenticationResponse>> Login(LoginRequest request)
  {
    throw new NotImplementedException();
  }
  public async Task<ApiResponse<string>> ResetDepartmentsAsync(long id)
  {
    ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var repository = _unitOfWork.Repository<User>();
    var user = await repository.GetAsyncTracking(
      id,
      q => q.Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
      .Include(u => u.UserGroups)
      .Include(u => u.UserDepartments));

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

  // Find users by expression with includes
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


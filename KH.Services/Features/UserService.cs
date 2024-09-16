using AutoMapper;
using Azure.Core;
using KH.Domain.Entities;
using KH.Dto.Models.UserDto.Response;
using KH.Helper.Extentions.Methods;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
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
  public Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request)
  {
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
  public Task<ApiResponse<AuthenticationResponse>> Login(AuthenticationLoginRequest request)
  {
    throw new NotImplementedException();
  }
  public async Task<ApiResponse<string>> ResetDepartmentsGetTrackedThenSaveAsync(List<long> request)
  {
    //get and try to check tracking then save 
    throw new NotImplementedException();
  }
}


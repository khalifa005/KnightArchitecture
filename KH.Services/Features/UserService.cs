using AutoMapper;
using KH.Domain.Entities;
using KH.Helper.Extentions.Methods;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
      .ThenInclude(x=> x.Group)
      .Include(u => u.UserDepartments)
      .ThenInclude(d=> d.Department));

    UserDetailsResponse userDetailsResponse = new UserDetailsResponse(detailsUserFromDB);
    //var userResponseByAutoMapper = _mapper.Map<UserDetailsResponse>(detailsUserFromDB);

    res.Data = userDetailsResponse;
    return res;
  }
  public Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request)
  {
    throw new NotImplementedException();
  }
  public Task<ApiResponse<UserListResponse>> GetListAsync(UserFilterRequest request)
  {
    throw new NotImplementedException();
  }

  public async Task<ApiResponse<string>> AddAsync(UserForm request)
  {
    //db context will handel saving it auto
    var actionMadeByUserId = _serviceProvider.GetUserId();

    //define our api res 
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

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


      await _unitOfWork.BeginTransactionAsync();

      var repository = _unitOfWork.Repository<User>();

      await repository.AddAsync(userEntityByAutoMapper);
      await _unitOfWork.CommitAsync();


      await _unitOfWork.CommitTransactionAsync();

      res.Data = userEntityByAutoMapper.Id.ToString();
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

  public async Task<ApiResponse<string>> UpdateAsync(UserForm request)
  {
    //define our api res 
    ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

    //there will be fluend validation rules
    if (request == null)
      throw new Exception("Invalid Parameter");

    //all validation should be in fluent validation side
    if (!request.Id.HasValue)
      throw new Exception("id is required");

    //auto mapper
    var userEntityByAutoMapper = _mapper.Map<User>(request);

    try
    {
      var repository = _unitOfWork.Repository<User>();

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

      await _unitOfWork.BeginTransactionAsync();


      repository.Update(userFromDb);
      await _unitOfWork.CommitAsync();
      await _unitOfWork.CommitTransactionAsync();

      res.Data = userEntityByAutoMapper.Id.ToString();
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

  public Task<ApiResponse<string>> DeleteAsync(UserFilterRequest request)
  {
    throw new NotImplementedException();
  }
  public Task<ApiResponse<AuthenticationResponse>> Login(AuthenticationLoginRequest request)
  {
    throw new NotImplementedException();
  }
}


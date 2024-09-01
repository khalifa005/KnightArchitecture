using AutoMapper;
using KH.Domain.Entities;
using KH.Helper.Extentions.Methods;
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


  public async Task<ApiResponse<string>> RegisterUserAsync(UserForm request)
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

      //check what happen to craetion date for user roles and user groups
      userEntityByAutoMapper.UserRoles = request.RoleIds.Select(roleId => new UserRole
      {
        RoleId = roleId
      }).ToList();

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

      //ignore this by the above i did define tem to be added auto to db 
      //--Save User Details with newly created Id
      //await this.SaveUserRoles(request, entity.Id);
      //await this.SaveUserDepartment(request, entity.Id);
      //await this.SaveUserGroup(request, entity.Id);
      //await _unitOfWork.CommitAsync();
      await _unitOfWork.CommitTransactionAsync();

      res.Data = "item.Data.Id";
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

  public Task<ApiResponse<string>> UpdateUserAsync(UserForm request)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<AuthenticationResponse>> Login(AuthenticationLoginRequest request)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<UserDetailsResponse>> GetUser(UserFilterRequest request)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<UserListResponse>> GetUserList(UserFilterRequest request)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<string>> DeleteUser(UserFilterRequest request)
  {
    throw new NotImplementedException();
  }
}


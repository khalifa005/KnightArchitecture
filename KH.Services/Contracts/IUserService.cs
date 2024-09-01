using KH.Dto.Models.AuthenticationDto.Request;
using KH.Dto.Models.AuthenticationDto.Response;
using KH.Dto.Models.UserDto.Form;
using KH.Dto.Models.UserDto.Request;
using KH.Dto.Models.UserDto.Response;
using KH.Helper.Responses;

namespace CA.Services.Contracts
{
  //: IService<UserDto, User, UserParameters, ApiResponse<UserDto>>
  public interface IUserService
  {
    Task<ApiResponse<string>> RegisterUserAsync(UserForm request);
    Task<ApiResponse<string>> UpdateUserAsync(UserForm request);
    Task<ApiResponse<AuthenticationResponse>> Login(AuthenticationLoginRequest request);
    Task<ApiResponse<UserDetailsResponse>> GetUser(UserFilterRequest request);
    Task<ApiResponse<UserListResponse>> GetUserList(UserFilterRequest request);
    Task<ApiResponse<string>> DeleteUser(UserFilterRequest request);

  }
}

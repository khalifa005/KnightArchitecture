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
    Task<ApiResponse<UserDetailsResponse>> GetAsync(long id);

    /// <summary>
    /// this will be use to get with multiple filteration option not just id
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request);
    Task<ApiResponse<UserListResponse>> GetListAsync(UserFilterRequest request);
    Task<ApiResponse<string>> AddAsync(UserForm request);
    Task<ApiResponse<string>> AddListAsync(List<UserForm> request);
    Task<ApiResponse<string>> UpdateAsync(UserForm request);
    Task<ApiResponse<string>> DeleteAsync(UserFilterRequest request);
    Task<ApiResponse<AuthenticationResponse>> Login(AuthenticationLoginRequest request);

  }
}

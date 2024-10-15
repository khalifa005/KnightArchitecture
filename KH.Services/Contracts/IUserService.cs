using System.Security.Claims;

namespace KH.Services.Contracts;

public interface IUserService
{
  Task<ApiResponse<UserDetailsResponse>> GetAsync(long id);
  Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListAsync(UserFilterRequest request);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingIQueryableAsync(UserFilterRequest request);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request);

  Task<ApiResponse<string>> AddAsync(UserForm request);
  Task<ApiResponse<string>> AddListAsync(List<UserForm> request);
  Task<ApiResponse<string>> UpdateAsync(UserForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<AuthenticationResponse>> LoginAsync(LoginRequest request);
  Task<ApiResponse<string>> ResetDepartmentsAsync(long id);
  Task<List<Claim>> GetUserClaims(LoginRequest request);
  Task<ApiResponse<AuthenticationResponse>> RefreshUserTokenAsync(string refreshTokenValue);

}

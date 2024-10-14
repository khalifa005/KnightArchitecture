namespace KH.Services.Users.Contracts;

public interface IUserQueryService
{
  Task<ApiResponse<UserDetailsResponse>> GetAsync(long id);
  Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListAsync(UserFilterRequest request);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingIQueryableAsync(UserFilterRequest request);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request);
  Task<ApiResponse<string>> CountUsersByAsync(UserFilterRequest request);
}

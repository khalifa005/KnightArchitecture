namespace KH.Services.Users.Contracts;

public interface IUserQueryService
{
  Task<ApiResponse<UserDetailsResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingIQueryableAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> CountUsersByAsync(UserFilterRequest request, CancellationToken cancellationToken);
}

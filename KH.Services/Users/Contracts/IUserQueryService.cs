using KH.Dto.Lookups.PermissionsDto.Response;

namespace KH.Services.Users.Contracts;

public interface IUserQueryService
{
  Task<ApiResponse<UserDetailsResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<UserDetailsResponse>> GetAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<PagedList<UserListResponse>>> GetListUsingIQueryableAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<PagedList<UserListResponse>>> GetListUsingProjectionAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> CountUsersByAsync(UserFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<List<PermissionResponse>>> GetUserPermissionsListAsync(CancellationToken cancellationToken);
}

using KH.Dto.Lookups.PermissionsDto.Request;
using KH.Dto.Lookups.PermissionsDto.Response;

namespace KH.Services.Lookups.Permissions.Contracts;

public interface IPermissionService
{
  Task<ApiResponse<PermissionResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddAsync(CreatePermissionRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreatePermissionRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<PermissionResponse>>> GetListAsync(CancellationToken cancellationToken);
}

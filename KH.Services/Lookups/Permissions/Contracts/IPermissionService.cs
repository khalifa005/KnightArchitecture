using KH.Dto.Lookups.PermissionsDto.Request;
using KH.Dto.Lookups.PermissionsDto.Response;

namespace KH.Services.Lookups.Permissions.Contracts;

public interface IPermissionService
{
  Task<ApiResponse<PermissionResponse>> GetAsync(long id);
  Task<ApiResponse<string>> AddAsync(CreatePermissionRequest request);
  Task<ApiResponse<string>> UpdateAsync(CreatePermissionRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<PagedResponse<PermissionResponse>>> GetListAsync();
}

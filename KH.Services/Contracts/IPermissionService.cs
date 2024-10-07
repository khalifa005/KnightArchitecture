using KH.Dto.lookups.PermissionDto.Form;
using KH.Dto.lookups.RoleDto.Form;
using KH.Dto.Lookups.PermissionsDto.Response;

namespace KH.Services.Contracts;

public interface IPermissionService
{
  Task<ApiResponse<PermissionResponse>> GetAsync(long id);
  Task<ApiResponse<string>> AddAsync(PermissionForm request);
  Task<ApiResponse<string>> UpdateAsync(PermissionForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<PagedResponse<PermissionResponse>>> GetListAsync();
}

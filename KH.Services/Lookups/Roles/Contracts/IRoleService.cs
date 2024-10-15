using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;

namespace KH.Services.Lookups.Roles.Contracts;

public interface IRoleService
{
  Task<ApiResponse<RoleResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<RoleListResponse>>> GetListAsync(RoleFilterRequest request);
  Task<ApiResponse<string>> AddAsync(CreateRoleRequest request);
  Task<ApiResponse<string>> UpdateAsync(CreateRoleRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<string>> UpdateBothRoleWithRelatedPermissionsAsync(CreateRoleRequest request);
  Task<ApiResponse<string>> UpdateRolePermissionsAsync(CreateRoleRequest request);
}

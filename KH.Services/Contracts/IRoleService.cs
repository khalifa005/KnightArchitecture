using KH.BuildingBlocks.Apis.Responses;
using KH.Dto.lookups.RoleDto.Form;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;

namespace KH.Services.Contracts;

public interface IRoleService
{
  Task<ApiResponse<RoleResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<RoleListResponse>>> GetListAsync(RoleFilterRequest request);
  Task<ApiResponse<string>> AddAsync(RoleForm request);
  Task<ApiResponse<string>> UpdateAsync(RoleForm request);
  Task<ApiResponse<string>> DeleteAsync(long id);
  Task<ApiResponse<string>> UpdateBothRoleWithRelatedPermissionsAsync(RoleForm request);
  Task<ApiResponse<string>> UpdateRolePermissionsAsync(RoleForm request);
}

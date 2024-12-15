using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;

namespace KH.Services.Lookups.Roles.Contracts;

public interface IRoleService
{
  Task<ApiResponse<RoleResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<RoleListResponse>>> GetPagedListAsync(RoleFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<List<RoleResponse>>> GetListAsync(RoleFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddAsync(CreateRoleRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreateRoleRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ReActivateAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateBothRoleWithRelatedPermissionsAsync(CreateRoleRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateRolePermissionsAsync(UpdatedRolePermissionsRequest request, CancellationToken cancellationToken);
}

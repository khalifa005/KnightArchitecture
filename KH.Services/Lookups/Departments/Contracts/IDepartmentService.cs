using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;
using KH.Dto.Lookups.RoleDto.Request;

namespace KH.Services.Lookups.Departments.Contracts;

public interface IDepartmentService
{
  Task<ApiResponse<DepartmentResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedList<DepartmentListResponse>>> GetPagedListAsync(DepartmentFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<List<DepartmentListResponse>>> GetListAsync(DepartmentFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ReActivateAsync(long id, CancellationToken cancellationToken);

}

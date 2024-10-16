using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;

namespace KH.Services.Lookups.Departments.Contracts;

public interface IDepartmentService
{
  Task<ApiResponse<DepartmentResponse>> GetAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<PagedResponse<DepartmentListResponse>>> GetListAsync(DepartmentFilterRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
}

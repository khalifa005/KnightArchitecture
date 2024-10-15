using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;

namespace KH.Services.Lookups.Departments.Contracts;

public interface IDepartmentService
{
  Task<ApiResponse<DepartmentResponse>> GetAsync(long id);
  Task<ApiResponse<PagedResponse<DepartmentListResponse>>> GetListAsync(DepartmentFilterRequest request);
  Task<ApiResponse<string>> AddAsync(CreateDepartmentRequest request);
  Task<ApiResponse<string>> UpdateAsync(CreateDepartmentRequest request);
  Task<ApiResponse<string>> DeleteAsync(long id);
}

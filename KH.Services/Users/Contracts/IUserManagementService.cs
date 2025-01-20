namespace KH.Services.Users.Contracts;

public interface IUserManagementService
{
  Task<ApiResponse<string>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> AddListAsync(List<CreateUserRequest> request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> UpdateAsync(CreateUserRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ResetDepartmentsAsync(long id, CancellationToken cancellationToken);
  Task<ApiResponse<string>> ReActivateAsync(long id, CancellationToken cancellationToken);

}

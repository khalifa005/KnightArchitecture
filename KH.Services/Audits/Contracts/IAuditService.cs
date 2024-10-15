namespace KH.Services.Audits.Contracts;

public interface IAuditService
{
  Task<ApiResponse<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId, CancellationToken cancellationToken);

  Task<ApiResponse<string>> ExportToExcelAsync(string userId, CancellationToken cancellationToken, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false );

  Task<ApiResponse<string>> ImportExternalAudit(IFormFile file, CancellationToken cancellationToken);

}

namespace KH.Services.Contracts;

public interface IPdfService
{
  Task<byte[]> ExportUserDetailsPdfAsync(UserFilterRequest request);

}



namespace KH.Services.Media_s.Contracts;

public interface IPdfService
{
  Task<byte[]> ExportUserDetailsPdfAsync(UserFilterRequest request);

}



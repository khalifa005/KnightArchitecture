using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.EmailDto.Response;

namespace KH.Services.Contracts;
public interface IEmailService
{
  //Task SendOrderConfirmationAsync(OrderConfirmationModel model);
  Task<ApiResponse<object>> SendEmailAsync(MailRequest mailRequest);
}



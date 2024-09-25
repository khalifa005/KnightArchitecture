using KH.Dto.Models.EmailDto.Response;

namespace KH.Services.Contracts;
public interface IEmailService
{
  Task SendOrderConfirmationAsync(OrderConfirmationModel model);
  Task SendWeeklyDigestAsync(WeeklyDigestModel model);
}



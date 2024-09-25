using FluentEmail.Core;
using KH.Dto.Models.EmailDto.Response;

public class EmailService : IEmailService
{
  private readonly IFluentEmail _fluentEmail;

  public EmailService(IFluentEmail fluentEmail)
  {
    _fluentEmail = fluentEmail;
  }

  // Method to send order confirmation email
  public async Task SendOrderConfirmationAsync(OrderConfirmationModel model)
  {
    string templatePath = $"{Directory.GetCurrentDirectory()}\\Templates\\Emails\\OrderConfirmation.cshtml";


    var email = _fluentEmail
        .To("mahmudkhalifa1@gmail.com", model.UserName)
        .Subject("Your Order Confirmation")
        .UsingTemplateFromFile(templatePath, model);

    // Attach invoice if necessary
    if (!string.IsNullOrEmpty(model.OrderId))
    {
      email.Attach(new FluentEmail.Core.Models.Attachment
      {
        //Data = new MemoryStream(File.ReadAllBytes($"Attachments/Invoice_{model.OrderId}.pdf")),
        Data = new MemoryStream(File.ReadAllBytes($"C:\\Application_Upload\\User_8\\09-25-2024-1-19-keyboard-shortcuts.pdf")),
        ContentType = "application/pdf",
        Filename = $"Invoice_{model.OrderId}.pdf"
      });
    }

    await email.SendAsync();
  }

  // Method to send weekly digest email
  public async Task SendWeeklyDigestAsync(WeeklyDigestModel model)
  {
    string templatePath = "Templates/Emails/WeeklyDigest.cshtml";

    var email = _fluentEmail
        .To(model.Email, model.UserName)
        .Subject("Your Weekly Digest")
        .UsingTemplateFromFile(templatePath, model);

    await email.SendAsync();
  }
}

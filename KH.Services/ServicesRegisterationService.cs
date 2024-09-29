using DinkToPdf;
using DinkToPdf.Contracts;
using KH.BuildingBlocks.Settings;
using KH.Services.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace KH.Services
{
  public static class ServicesRegisterationService
  {
    public static IServiceCollection AddBusinessService(this IServiceCollection services, IConfiguration configuration)
    {

      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IMediaService, MediaService>();
      services.AddScoped<IPdfService, PdfService>();
      services.AddScoped<IEmailService, EmailService>();

      // Register DinkToPdf converter
      services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

      //-- SET Fluent Email
      var mailSettings = configuration.GetSection("MailSettings");

      var mailOptions = mailSettings.Get<MailSettings>();
      var defaultFromEmail = mailOptions?.Mail ?? "crmesclations@acig.com.sa";
      var defaultHost = mailOptions?.Host ?? "130.90.4.184";
      var defaultPort = mailOptions?.Port ?? 25;

      // Add FluentEmail configuration
      services.AddFluentEmail(defaultFromEmail, mailOptions!.DisplayName)
          .AddRazorRenderer()
          .AddSmtpSender(new SmtpClient(mailOptions!.Host)
          {
            Port = 587,
            Credentials = new System.Net.NetworkCredential(mailOptions!.Mail, mailOptions!.Password),
            EnableSsl = true
          });



      return services;
    }
  }
}

using DinkToPdf;
using DinkToPdf.Contracts;
using KH.BuildingBlocks.Services;
using KH.BuildingBlocks.Settings;
using KH.Services.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace KH.Services;

public static class ServicesRegisterationService
{
  public static IServiceCollection AddBusinessService(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddIdentityService(configuration);

    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IMediaService, MediaService>();
    services.AddScoped<IPdfService, PdfService>();
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IRoleService, RoleService>();
    services.AddScoped<IDepartmentService, DepartmentService>();
    services.AddScoped<IGroupService, GroupService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IAuditService, AuditService>();
    services.AddSingleton<IExcelService, ExcelService>();

    // Register DinkToPdf converter
    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

    //-- SET Fluent Email
    var mailSettings = configuration.GetSection("MailSettings");

    var mailOptions = mailSettings.Get<MailSettings>();
    var defaultFromEmail = mailOptions?.Mail;
    var defaultHost = mailOptions?.Host;
    var defaultPort = mailOptions?.Port;

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

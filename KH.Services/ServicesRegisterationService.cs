using DinkToPdf;
using DinkToPdf.Contracts;
using KH.BuildingBlocks.Excel.Contracts;
using KH.BuildingBlocks.Excel.Services;
using KH.BuildingBlocks.Settings;
using KH.Services.BackgroundServices;
using KH.Services.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Simpl;

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
    services.AddScoped<IPermissionService, PermissionService>();
    services.AddScoped<ISmsTemplateService, SmsTemplateService>();
    services.AddScoped<ISmsService, SmsService>();

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

    services.AddQuartz(q =>
    {
      // Use a Scoped container to create jobs
      q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

      // Scheduler ID and Name for single instance
      q.SchedulerId = "AUTO";
      q.SchedulerName = "SingleScheduler"; // Give it a more suitable name for single instance
      // Misfire handling and recovery settings
      q.MisfireThreshold = TimeSpan.FromMinutes(1);  // 1-minute tolerance for missed jobs

      // Add job store TX (persistent job store)
      q.UsePersistentStore(options =>
      {
        // Perform schema validation
        options.PerformSchemaValidation = true;
        options.UseProperties = true; // Use properties
        options.UseSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection"));

        // Explicitly set JSON serializer
        options.UseNewtonsoftJsonSerializer();

        // Since it's a single instance, don't use clustering
        // Comment out the clustering options
        // options.UseClustering();
        // options.IsClustered = true;  // Disable clustering
        // options.ClusterCheckinInterval = TimeSpan.FromSeconds(20); // Not needed for single instance
      });

      var serviceProvider = services.BuildServiceProvider();
      var logger = serviceProvider.GetService<ILogger<EmailSenderJob>>();

      q.AddJobAndTrigger<EmailSenderJob>(configuration, logger);
    });


    services.AddQuartzHostedService(opt =>
    {
      opt.WaitForJobsToComplete = true;
    });

    return services;
  }
}

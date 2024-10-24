using KH.Services.Audits.Contracts;
using KH.Services.Audits.Implementation;
using KH.Services.Emails.Contracts;
using KH.Services.Lookups.Departments.Contracts;
using KH.Services.Lookups.Groups.Contracts;
using KH.Services.Lookups.Permissions.Contracts;
using KH.Services.Lookups.Roles.Contracts;
using KH.Services.Media_s.Contracts;
using KH.Services.Media_s.Implementation;
using KH.Services.Sms.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using KH.Dto.Models.UserDto.Validation;
using KH.Services.BackgroundJobs.QuartzJobs;
using KH.Services.BackgroundJobs.HangfireJobs.Contracts;
using KH.Services.BackgroundJobs.HangfireJobs.Implementation;
using Hangfire;
namespace KH.Services;

public static class ServicesRegistrationService
{
  public static IServiceCollection AddBusinessService(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddIdentityService(configuration);

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
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IUserManagementService, UserManagementService>();
    services.AddScoped<IUserQueryService, UserQueryService>();
    services.AddScoped<IUserValidationService, UserValidationService>();
    services.AddScoped<IEmailTrackerQueryService, EmailTrackerQueryService>();
    services.AddScoped<IUserNotificationService, UserNotificationService>();

    //background jobs
    services.AddScoped<IJobTestService, JobTestService>();

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
        options.UseSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection")!);

        // Explicitly set JSON serialize
        options.UseNewtonsoftJsonSerializer();
      });

      var serviceProvider = services.BuildServiceProvider();
      var logger = serviceProvider.GetService<ILogger<MissedEmailRetryJob>>();

      q.AddJobAndTrigger<MissedEmailRetryJob>(configuration, logger);
    });

    services.AddQuartzHostedService(opt =>
    {
      opt.WaitForJobsToComplete = true;
    });
    //hangfire

    services
      .AddHangfire(x =>
      {
        x.UseRecommendedSerializerSettings();
        x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
      });

    services.AddHangfireServer();

    // Register DinkToPdf converter
    services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

    // Add FluentValidation
    services.AddFluentValidationAutoValidation();
    services.AddValidatorsFromAssemblyContaining<CreateUserRequestListValidator>();

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

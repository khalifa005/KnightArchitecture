
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

    // Add Quartz services
    services.AddQuartz(q =>
    {
      // Use the extension method to add jobs and triggers from appsettings
      q.AddJobAndTrigger<ScheduleEmailSenderJob>(configuration);
    });
    services.AddQuartzHostedService(opt =>
    {
      opt.WaitForJobsToComplete = true;
    });

    //to use persistence store db
    //services.AddQuartz(q =>
    //{
    //  // Use a Scoped container to create jobs
    //  q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

    //  // Scheduler ID and Name for single instance
    //  q.SchedulerId = "AUTO";
    //  q.SchedulerName = "SingleScheduler"; 

    //  // Misfire handling and recovery settings
    //  q.MisfireThreshold = TimeSpan.FromMinutes(1);  // 1-minute tolerance for missed jobs

    //  // Add job store TX (persistent job store)
    //  q.UsePersistentStore(options =>
    //  {
    //    // Perform schema validation
    //    options.PerformSchemaValidation = true;
    //    options.UseProperties = true
    //    options.UseSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection")!);

    //    // Explicitly set JSON serialize
    //    options.UseNewtonsoftJsonSerializer();
    //  });

    //  var serviceProvider = services.BuildServiceProvider();
    //  var logger = serviceProvider.GetService<ILogger<MissedEmailRetryJob>>();

    //  q.AddJobAndTrigger<MissedEmailRetryJob>(configuration, logger);
    //});



    //Register Hangfire
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

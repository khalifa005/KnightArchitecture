using KH.Helper.Extentions;
using KH.Helper.Extentions.Files;
using KH.Helper.Middlewares;
using KH.Helper.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace KH.Helper
{
  public static class HelperServiceRegisteration
  {
    public static IServiceCollection AddHelperServicesAndSettings(this IServiceCollection services, IConfiguration configuration)
    {
      #region Settings

      //use options patterns if u want to add validation while startup pn the json files
      //or reload the values when file changes
      var globalSettingSection = configuration.GetSection("GlobalSettings");
      services.Configure<GlobalSettings>(globalSettingSection);

      //var fileSettingsSection = configuration.GetSection("FileSettings");
      //services.Configure<FileSettings>(fileSettingsSection);
      services.AddOptions<FileSettings>()
        .Bind(configuration.GetSection("FileSettings"))
        .ValidateDataAnnotations(); // Optional validation if using annotations


      var tokenSettingsSection = configuration.GetSection("TokenSettings");
      services.Configure<TokenSettings>(tokenSettingsSection);

      var smsProviderSettings = configuration.GetSection("SMSProviderSettings");
      services.Configure<SMSProviderSettings>(smsProviderSettings);


      var mailTemplateSettings = configuration.GetSection("MailTemplatesSettings");
      services.Configure<MailTemplatesSettings>(mailTemplateSettings);


      //-- SET Fluent Email
      var mailSettings = configuration.GetSection("MailSettings");

      services.AddOptions<MailSettings>()
       .Bind(mailSettings)
       .ValidateDataAnnotations();

      #endregion

      //services.AddScoped(typeof(Lazy<>), typeof(LazilyResolved<>));

      services.AddScoped<FileManager>();
      //services.AddSingleton<FileValidator>();

      services.AddSwaggerDocumentation(configuration);


      return services;
    }

    public static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
    {
      // keep it empty because this part we want to keep it on the startup for better viewing no encasulation
      //because also middleware order matters

      //app.UseMiddleware<ExceptionMiddleware>();
      //app.UseSwaggerDocumentation(configuration);


      return app;
    }
  }
}

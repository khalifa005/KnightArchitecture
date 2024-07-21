
using CA.Application.Helpers;
using KH.Helper.Extentions;
using KH.Helper.Middlewares;
using KH.Helper.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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

      var fileSettingsSection = configuration.GetSection("FileSettings");
      services.Configure<FileSettings>(fileSettingsSection);

      var tokenSettingsSection = configuration.GetSection("TokenSettings");
      services.Configure<TokenSettings>(tokenSettingsSection);

      var smsProviderSettings = configuration.GetSection("SMSProviderSettings");
      services.Configure<SMSProviderSettings>(smsProviderSettings);

      var mailSettings = configuration.GetSection("MailSettings");
      var options = services.Configure<MailSettings>(mailSettings);

      var mailTemplateSettings = configuration.GetSection("MailTemplatesSettings");
      services.Configure<MailTemplatesSettings>(mailTemplateSettings);


      #endregion

      //services.AddScoped(typeof(Lazy<>), typeof(LazilyResolved<>));

      services.AddScoped<FileManager>();

			services.AddSwaggerDocumentation(configuration);

			//-- SET Fluent Email
			var mailOptions = mailSettings.Get<MailSettings>();
			var defaultFromEmail = mailOptions?.Mail ?? "crmesclations@acig.com.sa";
			var defaultHost = mailOptions?.Host ?? "130.90.4.184";
			var defaultPort = mailOptions?.Port ?? 25;

     // services.AddFluentEmail(defaultFromEmail)
					//.AddRazorRenderer()
					//.AddSmtpSender(defaultHost, defaultPort);

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

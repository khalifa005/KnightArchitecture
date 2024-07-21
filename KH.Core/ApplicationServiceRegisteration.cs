
using CA.Application.Helpers;
using KH.Helper.Extentions;
using KH.Helper.Middlewares;
using KH.Helper.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace KH.Helper
{
	public static class ApplicationServiceRegisteration
	{
		public static IServiceCollection AddApplicationServiceAndSettings(this IServiceCollection services, IConfiguration configuration)
		{

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

			//services.AddScoped(typeof(Lazy<>), typeof(LazilyResolved<>));

			services.AddScoped<FileManager>();

			//services.Configure<ApiBehaviorOptions>(options =>
			//{
			//	options.InvalidModelStateResponseFactory = actionContext =>
			//	{
			//		var modelState = actionContext.ModelState
			//		.Select(p => new { key = p.Key, errors = p.Value.Errors.Select(e => e.ErrorMessage) })
			//		.ToDictionary(kv => kv.key, kv => kv.errors);

			//		var errorResponse = new ApiValidationError
			//		{
			//			ErrorDetails = modelState
			//		};

			//		return new BadRequestObjectResult(errorResponse.HandleApiResponseError());
			//	};
			//});

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
			//app.UseMiddleware<ExceptionMiddleware>();

			//app.UseSwaggerDocumentation(configuration);


			return app;
		}
	}
}

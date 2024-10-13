using KH.BuildingBlocks.Constant;
using KH.BuildingBlocks.Extentions.Files;
using KH.BuildingBlocks.Extentions.Methods;
using KH.BuildingBlocks.Services;
using KH.BuildingBlocks.Settings;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace KH.BuildingBlocks;

public static class HelperServiceRegisteration
{
  public static IServiceCollection AddHelperServicesAndSettings(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddHttpContextAccessor();
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
    services.AddScoped<FileManager>();
    //services.AddScoped(typeof(Lazy<>), typeof(LazilyResolved<>));
    services.RegisterSwagger();
    services.AddServerLocalization();

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
    services.Configure<SmsSettings>(smsProviderSettings);


    var mailTemplateSettings = configuration.GetSection("MailTemplatesSettings");
    services.Configure<MailTemplatesSettings>(mailTemplateSettings);


    //-- SET Fluent Email
    var mailSettings = configuration.GetSection("MailSettings");

    services.AddOptions<MailSettings>()
     .Bind(mailSettings)
     .ValidateDataAnnotations();

    var SmsSettings = configuration.GetSection("SmsSettings");
    services.AddOptions<SmsSettings>()
     .Bind(SmsSettings)
     .ValidateDataAnnotations();

    #endregion

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

  internal static async Task<IStringLocalizer> GetRegisteredServerLocalizerAsync<T>(this IServiceCollection services) where T : class
  {
    var serviceProvider = services.BuildServiceProvider();
    await SetCultureFromServerPreferenceAsync(serviceProvider);
    var localizer = serviceProvider.GetService<IStringLocalizer<T>>();
    await serviceProvider.DisposeAsync();
    return localizer;
  }

  private static async Task SetCultureFromServerPreferenceAsync(IServiceProvider serviceProvider)
  {
    var storageService = serviceProvider.GetService<ServerPreferenceManager>();
    if (storageService != null)
    {
      // TODO - should implement ServerStorageProvider to work correctly!
      CultureInfo culture;
      if (await storageService.GetPreference() is ServerPreference preference)
        culture = new(preference.LanguageCode);
      else
        culture = new(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US");
      CultureInfo.DefaultThreadCurrentCulture = culture;
      CultureInfo.DefaultThreadCurrentUICulture = culture;
      CultureInfo.CurrentCulture = culture;
      CultureInfo.CurrentUICulture = culture;
    }
  }

  internal static IServiceCollection AddServerLocalization(this IServiceCollection services)
  {
    services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ServerLocalizer<>));
    return services;
  }


}

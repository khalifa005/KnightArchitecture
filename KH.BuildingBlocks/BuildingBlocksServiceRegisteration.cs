

namespace KH.BuildingBlocks;

public static class BuildingBlocksServiceRegisteration
{
  public static IServiceCollection AddBuildingBlocksServices(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddHttpContextAccessor();
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
    services.AddScoped<FileManagerService>();
    services.RegisterSwagger();
    services.AddServerLocalization();
    //services.AddScoped(typeof(Lazy<>), typeof(LazilyResolved<>));


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

  public static IApplicationBuilder UseBuildingBlocksMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
  {

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

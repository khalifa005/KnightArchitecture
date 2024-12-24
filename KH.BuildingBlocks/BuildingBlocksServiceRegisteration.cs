

using KH.BuildingBlocks.Cache.Enums;
using KH.BuildingBlocks.Cache.Interfaces;
using QuestPDF.Infrastructure;

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
    //services.AddScoped(typeof(Lazy<>), typeof(LazilyResolved<>));

    #region KeyedServices
    services.AddKeyedTransient<ICacheService, MemoryCacheService>(CacheTechEnum.Memory);
    services.AddKeyedTransient<ICacheService, RedisCacheService>(CacheTechEnum.Redis);
    #endregion


    #region Settings

    var globalSettingSection = configuration.GetSection("GlobalSettings");
    services.Configure<GlobalSettings>(globalSettingSection);

    services.AddOptions<FileSettings>()
      .Bind(configuration.GetSection("FileSettings"))
      .ValidateDataAnnotations();

    var tokenSettingsSection = configuration.GetSection("TokenSettings");
    services.Configure<TokenSettings>(tokenSettingsSection);

    var smsProviderSettings = configuration.GetSection("SMSProviderSettings");
    services.Configure<SmsSettings>(smsProviderSettings);


    var mailTemplateSettings = configuration.GetSection("MailTemplatesSettings");
    services.Configure<MailTemplatesSettings>(mailTemplateSettings);

    var mailSettings = configuration.GetSection("MailSettings");

    services.AddOptions<MailSettings>()
     .Bind(mailSettings)
     .ValidateDataAnnotations();

    var SmsSettings = configuration.GetSection("SmsSettings");
    services.AddOptions<SmsSettings>()
     .Bind(SmsSettings)
     .ValidateDataAnnotations();

    var CacheSettings = configuration.GetSection("CacheSettings");
    services.AddOptions<CacheSettings>()
     .Bind(CacheSettings)
     .ValidateDataAnnotations();

    #endregion


    #region Packages License
    // Configure QuestPDF license
    QuestPDF.Settings.License = LicenseType.Community;

    #endregion

    return services;
  }

  public static IApplicationBuilder UseBuildingBlocksMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
  {

    return app;
  }


}


using KH.BuildingBlocks.Contracts.Persistence;
using KH.PersistenceInfra.Data;
using KH.PersistenceInfra.Middlewares;
using KH.PersistenceInfra.Repositories;
using KH.PersistenceInfra.Services;

namespace KH.PersistenceInfra;

public static class InfrastructureServiceRegisteration
{
  public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddDbContext<AppDbContext>(db => db.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    //services.AddSingleton<DapperContext>();

    //services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    //services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    //token setting registration
    //services.AddIdentityService(configuration);
    //services.AddDistributedMemoryCache();
    //services.AddSingleton<IConnectionMultiplexer>(c =>
    //{
    //  ConfigurationOptions config = new ConfigurationOptions()
    //  {
    //    SyncTimeout = 500000,
    //    EndPoints = { configuration.GetConnectionString("RedisConnection") },
    //    AbortOnConnectFail = false // this prevents that error
    //  };
    //  return ConnectionMultiplexer.Connect(config);
    //});

    services.AddCors(opt =>
    {
      opt.AddPolicy("CorsPolicy", policy =>
            {
              //var issuer = configuration["TokenSettings:Issuer"];
              //if (issuer != null)
              //    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(issuer);
              //else
              policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
    });

    //services.AddSession(options =>
    //{
    //  options.IdleTimeout = TimeSpan.FromMinutes(30);
    //});

    return services;
  }

  public static IApplicationBuilder UseInfrastructureMiddleware(this IApplicationBuilder app, IConfiguration configuration)
  {
    app.MigrateDatabase<AppDbContext>((context, services) =>
    {
      var logger = services.GetRequiredService<ILogger<AppDbContext>>();
      //CleanArchitectDbContext.SeedAsync(context, logger).Wait();
    });


    return app;
  }
}

using Hangfire;
using KH.PersistenceInfra.Data;
using KH.PersistenceInfra.Data.Interceptors;
using KH.PersistenceInfra.Middlewares;
using KH.PersistenceInfra.Repositories;
using KH.PersistenceInfra.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KH.PersistenceInfra;

public static class InfrastructureServiceRegisteration
{
  public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
  {
    //services.AddScoped<AuditLoggingInterceptor>();

    services.AddDbContext<AppDbContext>((sp, db) =>
    db.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
    .AddInterceptors(
      //sp.GetRequiredService<AuditLoggingInterceptor>()
      )
    );
    //services.AddSingleton<DapperContext>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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

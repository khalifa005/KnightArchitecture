
using KH.BuildingBlocks.Auth.V1;
using KH.BuildingBlocks.Contracts.Persistence;
using KH.PersistenceInfra.Data;
using KH.PersistenceInfra.Middlewares;
using KH.PersistenceInfra.Repositories;
using KH.PersistenceInfra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace KH.PersistenceInfra;

public static class InfrastructureServiceRegisteration
{
  public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddDbContext<AppDbContext>(db => db.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
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


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

    //services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    //services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    services.AddScoped<IUserPermissionService, UserPermissionService>();
    services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

    services.AddIdentityService(configuration);

   
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


using CA.ViewModels;
using KH.Helper;
using KH.Helper.Extentions;
using KH.Helper.Middlewares;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using FluentValidation.AspNetCore;

using FluentValidation;
using KH.PersistenceInfra;
using KH.Services;
using KH.Helper.Responses;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Microsoft.Extensions.Hosting.Internal;
namespace KH.WebApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
      Configuration = configuration;
      Env = hostingEnvironment;
    }

    public IConfiguration Configuration { get; }
    IWebHostEnvironment Env { get; }

    public void ConfigureServices(IServiceCollection services)
    {

      // Add services to the container.
      services.AddHttpContextAccessor();

      services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.AddEndpointsApiExplorer();

      services.AddFluentValidationAutoValidation();
      services.AddValidatorsFromAssemblyContaining<Startup>();

      services.AddControllers()
          .ConfigureApiBehaviorOptions(options =>
          {
            options.InvalidModelStateResponseFactory = context =>
            {
              var errors = context.ModelState
                  .Where(x => x.Value.Errors.Count > 0)
                  .ToDictionary(
                      kvp => kvp.Key,
                      kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                  );

              var customErrorResponse = new ApiResponse<object>(400)
              {
                ErrorMessage = "validation-failed.",
                Errors = errors.SelectMany(x => x.Value).ToList()
              };

              return new BadRequestObjectResult(customErrorResponse);
            };
          })
              .AddJsonOptions(options =>
              {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
              })
              .AddNewtonsoftJson(options =>
              {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

              }
        );

      services.AddMemoryCache();


      services.AddInfrastructureService(Configuration);
      //services.AddBusinessService(Configuration);
      services.AddDtoService(Configuration);
      //contains the common service registration
      services.AddHelperServicesAndSettings(Configuration);
      services.AddBusinessService(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
      //app.UseMiddleware<ValidationErrorMiddleware>();

      app.UseStatusCodePagesWithReExecute("/errors/{0}");

      app.UseInfrastructureMiddleware(Configuration);
      //contains multiple middileware
      //app.UseApplicationMiddlewares(Configuration);

      app.UseMiddleware<ExceptionMiddleware>();

      app.UseSwaggerDocumentation(Configuration);

      if (env.IsDevelopment())
      {

      }
      else
      {
        //app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseCors("CorsPolicy");
      app.UseHttpsRedirection();

      app.UseStaticFiles();
      //app.UseStaticFiles(new StaticFileOptions()
      //{
      //  FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
      //  RequestPath = new PathString("/Resources")
      //});

      app.UseRouting();
      app.UseAuthentication();
      // order here matters - after UseAuthentication so we have the Identity populated in the HttpContext
      //app.UseMiddleware<PermissionsMiddleware>();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });



    }
  }


}

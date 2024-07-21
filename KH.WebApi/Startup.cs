
using KH.Helper;
using KH.Helper.Extentions;
using KH.Helper.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

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
      services
          .AddControllersWithViews()
          .AddJsonOptions(options =>
          {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

          }

           );
      //.AddFluentValidation(fv =>
      //{
      //    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
      //});


      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
      //services.AddApplicationService(Configuration);
      //services.AddInfrastructureService(Configuration);
      //services.AddViewModelService(Configuration);
      //services.AddBusinessService(Configuration);

      services.AddEndpointsApiExplorer();
      //services.AddSwaggerDocumentation(Configuration);
      //contains the common service registration
      services.AddApplicationServiceAndSettings(Configuration);


    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

      app.UseStatusCodePagesWithReExecute("/errors/{0}");

      //app.UseInfrastructureMiddleware(Configuration);
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

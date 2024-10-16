//using FluentValidation;
//using FluentValidation.AspNetCore;
//using KH.BuildingBlocks;
//using KH.BuildingBlocks.Apis.Extentions;
//using KH.BuildingBlocks.Apis.Middlewares;
//using KH.BuildingBlocks.Apis.Services;
//using KH.BuildingBlocks.Auth.Midilleware;
//using KH.Dto;
//using KH.Dto.Models.UserDto.Validation;
//using KH.Dto.Validations;
//using KH.PersistenceInfra;
//using KH.Services;
//namespace KH.WebApi;

//public class Startup
//{
//  public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
//  {
//    Configuration = configuration;
//    Env = hostingEnvironment;
//  }

//  public IConfiguration Configuration { get; }
//  IWebHostEnvironment Env { get; }

//  public void ConfigureServices(IServiceCollection services)
//  {

//    // Add services to the container.

//    services.AddHttpClient();
//    services.AddTransient<CustomHttpRequestService>(); // Register HttpRequestHelper as a service

//    services.AddEndpointsApiExplorer();
//    services.AddFluentValidationAutoValidation();
//    services.AddValidatorsFromAssemblyContaining<Startup>();

//    services.AddControllers(options =>
//    {
//        // Disable ASP.NET Core's default model validation
//        //options.ModelValidatorProviders.Clear();
//    }).ConfigureApiBehaviorOptions(options =>
//        {
//          options.InvalidModelStateResponseFactory = context =>
//          {
//            var errors = context.ModelState
//                .Where(x => x.Value.Errors.Count > 0)
//                .ToDictionary(
//                    kvp => kvp.Key,
//                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
//                );
//            // Log the raw model state errors for further inspection
//            var detailedErrors = context.ModelState.SelectMany(x => x.Value.Errors.Select(e => e.Exception?.Message ?? e.ErrorMessage)).ToList();

//            // Log these errors to check if something else is affecting validation
//            Console.WriteLine("ModelState Errors: " + string.Join(", ", detailedErrors));

//            var customErrorResponse = new ApiResponse<object>(400)
//            {
//              ErrorMessage = "validation-failed.",
//              Errors = errors.SelectMany(x => x.Value).ToList()
//            };

//            return new BadRequestObjectResult(customErrorResponse);
//          };
//        })
//            .AddJsonOptions(options =>
//            {
//              options.JsonSerializerOptions.PropertyNamingPolicy = null;
//              options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//            })
//            .AddNewtonsoftJson(options =>
//            {
//              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

//            }
//      );

//    services.AddMemoryCache();
//    //services.AddSession(options =>
//    //{
//    //  options.IdleTimeout = TimeSpan.FromMinutes(30);
//    //});

//    //services.AddSingleton<IResponseCacheService, ResponseCacheService>();

//    services.AddCors(opt =>
//    {
//      opt.AddPolicy("CorsPolicy", policy =>
//      {
//        //var issuer = configuration["TokenSettings:Issuer"];
//        //if (issuer != null)
//        //    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(issuer);
//        //else
//        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
//      });
//    });

//    services.AddInfrastructureService(Configuration);
//    //services.AddBusinessService(Configuration);
//    services.AddDtoService(Configuration);
//    //contains the common service registration
//    services.AddBuildingBlocksServices(Configuration);
//    services.AddBusinessService(Configuration);
//  }

//  public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
//  {
//    System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

//    // Add Correlation ID Middleware early in the pipeline
//    app.UseMiddleware<CorrelationIdMiddleware>();

//    app.UseStatusCodePagesWithReExecute("/errors/{0}");

//    app.UseInfrastructureMiddleware(Configuration);
//    //contains multiple middileware
//    //app.UseApplicationMiddlewares(Configuration);

//    app.UseMiddleware<ExceptionMiddleware>();

//    app.UseSwaggerDocumentationMiddleware(Configuration);

//    if (env.IsDevelopment())
//    {

//    }
//    else
//    {
//      //app.UseExceptionHandler("/Home/Error");
//      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//      app.UseHsts();
//    }

//    app.UseCors("CorsPolicy");
//    app.UseHttpsRedirection();
//    app.UseRouting();

//    app.UseAuthentication();
//    // order here matters - after UseAuthentication so we have the Identity populated in the HttpContext
//    app.UseMiddleware<PermissionsMiddleware>();
//    app.UseAuthorization();

//    app.UseStaticFiles();
//    //app.UseStaticFiles(new StaticFileOptions()
//    //{
//    //  FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
//    //  RequestPath = new PathString("/Resources")
//    //});




//    app.UseEndpoints(endpoints =>
//    {
//      endpoints.MapControllers();
//    });



//  }

//}

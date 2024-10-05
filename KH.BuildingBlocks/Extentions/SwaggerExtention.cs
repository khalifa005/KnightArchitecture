using Microsoft.OpenApi.Models;

namespace KH.BuildingBlocks.Extentions;

public static class SwaggerExtention
{
  internal static void RegisterSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(c =>
    {
      // Include all project's XML comments
      var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (!assembly.IsDynamic)
        {
          var xmlFile = $"{assembly.GetName().Name}.xml";
          var xmlPath = Path.Combine(baseDirectory, xmlFile);
          if (File.Exists(xmlPath))
          {
            c.IncludeXmlComments(xmlPath);
          }
        }
      }

      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Version = "v1",
        Title = "CleanArchitecture",
        License = new OpenApiLicense
        {
          Name = "MIT License",
          Url = new Uri("https://opensource.org/licenses/MIT")
        }
      });

      // JWT Bearer Authentication
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
      });

      // Basic Authentication
      c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Input your username and password to access this API"
      });

      // Add Security Requirements for both JWT and Basic Auth
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            },
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Basic"
                    },
                    Scheme = "basic",
                    Name = "Basic",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    });
  }


  public static IApplicationBuilder UseSwaggerDocumentationMiddleware(this IApplicationBuilder app, IConfiguration configuration)
  {

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
      var isLocal = bool.TryParse(configuration["GlobalSettings:IsLocal"], out bool local) && local;
      var iisApiName = configuration["GlobalSettings:IISApiName"];
      var swagV1 = isLocal ? "/swagger/v1/swagger.json" : $"/{iisApiName}/swagger/v1/swagger.json";

      // Set Swagger endpoint based on environment
      options.SwaggerEndpoint(swagV1, isLocal ? "My API" : "Clean Architecture Web API V1");

      // Set route and additional UI options
      options.RoutePrefix = "swagger";
      options.DisplayRequestDuration();
      options.DefaultModelsExpandDepth(-1);  // Collapse models by default
    });

    return app;
  }
}


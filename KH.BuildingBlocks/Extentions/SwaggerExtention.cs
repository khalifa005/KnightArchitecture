using Microsoft.OpenApi.Models;

namespace KH.BuildingBlocks.Extentions;

public static class SwaggerExtention
{
  public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddSwaggerGen();

    //services.AddSwaggerGen(options =>
    //{
    //  options.SwaggerDoc("v1", new OpenApiInfo { Title = "Clean Architecture Web API V1", Version = "version 1" });

    //  // bearer -> basic , Bearer -> Basic

    //  //var securitySchema = new OpenApiSecurityScheme
    //  //{
    //  //    Description = "Jwt Auth Basic Scheme",
    //  //    Name = "Authorization",
    //  //    In = ParameterLocation.Header,
    //  //    Type = SecuritySchemeType.Http,
    //  //    Scheme = "basic",
    //  //    Reference = new OpenApiReference
    //  //    {
    //  //        Id = "Basic",
    //  //        Type = ReferenceType.SecurityScheme
    //  //    }
    //  //};

    //  //var securitySchema = new OpenApiSecurityScheme
    //  //{
    //  //  Description = "identity",
    //  //  Name = "Authorization",
    //  //  In = ParameterLocation.Header,
    //  //  Type = SecuritySchemeType.ApiKey,
    //  //  Scheme = "oauth2",
    //  //  Reference = new OpenApiReference
    //  //  {
    //  //    Id = "Bearer",
    //  //    Type = ReferenceType.SecurityScheme
    //  //  }
    //  //};

    //  //options.AddSecurityDefinition("Bearer", securitySchema);
    //  //var securityRequirement = new OpenApiSecurityRequirement
    //  //  {
    //  //            { securitySchema , new[] { "Bearer" } }
    //  //  };

    //  //options.AddSecurityDefinition("Basic", securitySchema);
    //  //var securityRequirement = new OpenApiSecurityRequirement
    //  //{
    //  //    { securitySchema , new[] { "Basic" } }
    //  //};
    //  //options.AddSecurityRequirement(securityRequirement);

    //});


    return services;
  }

  internal static void RegisterSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(async c =>
    {
      //TODO - Lowercase Swagger Documents
      //c.DocumentFilter<LowercaseDocumentFilter>();
      //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f

      // include all project's xml comments
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

      //var localizer = await GetRegisteredServerLocalizerAsync<ServerCommonResources>(services);

      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
        //Description = localizer["Input your Bearer token in this format - Bearer {your token here} to access this API"],
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
    });
  }
}


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
}

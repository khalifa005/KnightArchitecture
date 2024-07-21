//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace CA.Application.Extentions
//{
//    public static class SwaggerExtention
//    {
//        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddApiVersioning(options =>
//            {
//                options.ReportApiVersions = true;
//                options.AssumeDefaultVersionWhenUnspecified = true;
//                options.DefaultApiVersion = new ApiVersion(1, 0);
//                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
//                //options.ErrorResponses = new MyErrorResponseProvider();
//            });

//            services.AddVersionedApiExplorer(options =>
//            {
//                options.GroupNameFormat = "'v'VVV";
//            });

//            services.AddSwaggerGen(options =>
//            {
//                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Clean Architecture Web API V1", Version = "version 1" });
//                options.SwaggerDoc("v2", new OpenApiInfo { Title = "Clean Architecture Web API V2", Version = "version 2" });

//                // bearer -> basic , Bearer -> Basic

//                //var securitySchema = new OpenApiSecurityScheme
//                //{
//                //    Description = "Jwt Auth Basic Scheme",
//                //    Name = "Authorization",
//                //    In = ParameterLocation.Header,
//                //    Type = SecuritySchemeType.Http,
//                //    Scheme = "basic",
//                //    Reference = new OpenApiReference
//                //    {
//                //        Id = "Basic",
//                //        Type = ReferenceType.SecurityScheme
//                //    }
//                //};

//                var securitySchema = new OpenApiSecurityScheme
//                {
//                    Description = "identity",
//                    Name = "Authorization",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.ApiKey,
//                    Scheme = "oauth2",
//                    Reference = new OpenApiReference
//                    {
//                        Id = "Bearer",
//                        Type = ReferenceType.SecurityScheme
//                    }
//                };

//                options.AddSecurityDefinition("Bearer", securitySchema);
//                var securityRequirement = new OpenApiSecurityRequirement
//                {
//                    { securitySchema , new[] { "Bearer" } }
//                };

//                //options.AddSecurityDefinition("Basic", securitySchema);
//                //var securityRequirement = new OpenApiSecurityRequirement
//                //{
//                //    { securitySchema , new[] { "Basic" } }
//                //};
//                options.AddSecurityRequirement(securityRequirement);

//            });


//            return services;
//        }
//    }
//}

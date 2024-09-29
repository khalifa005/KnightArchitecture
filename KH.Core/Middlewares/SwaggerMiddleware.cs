namespace KH.BuildingBlocks.Middlewares
{
  public static class SwaggerMiddleware
  {
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
    {

      app.UseSwagger();
      app.UseSwaggerUI();

      //var isLocal = Convert.ToBoolean(configuration["GlobalSettings:IsLocal"]);
      //var iisApiName = configuration["GlobalSettings:IISApiName"];

      //string swagV1 = "/swagger/v1/swagger.json";

      //if (isLocal == false && !string.IsNullOrEmpty(iisApiName))
      //{
      //    swagV1 = "/" + iisApiName + swagV1;
      //}

      //app.UseSwagger();

      //app.UseSwaggerUI(options =>
      //{
      //    options.DefaultModelsExpandDepth(-1);
      //    options.SwaggerEndpoint(swagV1, "Clean Architecture Web API V1");
      //});

      return app;
    }
  }
}


namespace KH.PersistenceInfra.Middlewares
{
  public static class MigrateDBMiddleware
  {
    public static IApplicationBuilder MigrateDatabase<TContext>(this IApplicationBuilder app, Action<TContext, IServiceProvider> seeder, int? retry = 0)
        where TContext : DbContext
    {

      int retryForAvailability = retry.Value;

      using (var scope = app.ApplicationServices.CreateScope())
      {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
          logger.LogInformation("Migrating database associated with context {DbContextnNme}", typeof(TContext).Name);

          InvokeSeeder<TContext>(seeder, context, services);

          logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "An error occured while migrating the database used on context {DbContextName}", typeof(TContext).Name);

          if (retryForAvailability < 50)
          {
            retryForAvailability++;
            System.Threading.Thread.Sleep(2000);
            MigrateDatabase<TContext>(app, seeder, retryForAvailability);
          }
        }
      }

      return app;
    }

    static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
                                       TContext context,
                                       IServiceProvider services)
        where TContext : DbContext
    {
      context.Database.Migrate();
      seeder(context, services);
    }
  }
}

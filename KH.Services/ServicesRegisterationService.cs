using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KH.Services
{
  public static class ServicesRegisterationService
  {
    public static IServiceCollection AddBusinessService(this IServiceCollection services, IConfiguration configuration)
    {

      services.AddScoped<IUserService, UserService>();

      return services;
    }
  }
}

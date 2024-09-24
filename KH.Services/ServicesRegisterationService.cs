using KH.Services.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KH.Services
{
  public static class ServicesRegisterationService
  {
    public static IServiceCollection AddBusinessService(this IServiceCollection services, IConfiguration configuration)
    {

      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IMediaService, MediaService>();

      return services;
    }
  }
}

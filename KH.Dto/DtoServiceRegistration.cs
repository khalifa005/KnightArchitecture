
using CA.ViewModels.MappingProfiles;
using FluentValidation;
using KH.Dto.lookups.Group.Response;
using KH.Dto.lookups.Group.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace CA.ViewModels
{
  public static class DtoServiceRegistration
    {
        public static IServiceCollection AddDtoService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            services.AddSingleton<IValidator<CityResponse>, CityFormValidator>();

            return services;
        }
    }
}

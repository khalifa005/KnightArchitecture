
using CA.ViewModels.MappingProfiles;
using CA.ViewModels.Validations;
using FluentValidation;
using KH.Dto.Models.lookups;
using Microsoft.Extensions.DependencyInjection;

namespace CA.ViewModels
{
    public static class DtoServiceRegistration
    {
        public static IServiceCollection AddDtoService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            services.AddSingleton<IValidator<GroupDto>, GroupValidator>();

            return services;
        }
    }
}

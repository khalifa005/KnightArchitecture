
using CA.ViewModels.MappingProfiles;
using FluentValidation;
using KH.Dto.lookups.CityDto.Form;
using KH.Dto.lookups.CityDto.Request;
using KH.Dto.lookups.CityDto.Validation;
using KH.Dto.lookups.Group.Validation;
using KH.Dto.lookups.GroupDto.Form;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Validation;
using KH.Dto.Models.AuthenticationDto.Request;
using KH.Dto.Models.OtpVerificationDto.Validation;

namespace CA.ViewModels
{
  public static class DtoServiceRegistration
  {
    public static IServiceCollection AddDtoService(this IServiceCollection services, IConfiguration configuration)
    {
      //automapper
      services.AddAutoMapper(typeof(AutoMapperConfiguration));

      //below validation registration using fluent validation lib
      services.AddSingleton<IValidator<CityForm>, DepartmentFormValidator>();
      services.AddSingleton<IValidator<CityFilterRequest>, CityFilterRequestValidator>();

      services.AddSingleton<IValidator<GroupForm>, GroupFormValidator>();
      services.AddSingleton<IValidator<GroupFilterRequest>, GroupFilterRequestValidator>();

      services.AddSingleton<IValidator<OtpVerificationRequest>, OtpVerificationRequestValidator>();

      return services;
    }
  }
}

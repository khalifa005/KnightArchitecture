using FluentValidation;
using KH.Dto.lookups.CityDto.Form;
using KH.Dto.lookups.CityDto.Request;
using KH.Dto.lookups.CityDto.Validation;
using KH.Dto.lookups.GroupDto.Form;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Validation;
using KH.Dto.Lookups.GroupDto.Validation;
using KH.Dto.MappingProfiles;
using KH.Dto.Models.AuthenticationDto.Request;
using KH.Dto.Models.AuthenticationDto.Validation;
using KH.Dto.Models.CustomerDto.Form;
using KH.Dto.Models.CustomerDto.Validation;
using KH.Dto.Models.MediaDto.Form;
using KH.Dto.Models.MediaDto.Validation;
using KH.Dto.Models.UserDto.Form;
using KH.Dto.Models.UserDto.Validation;

namespace KH.Dto;

public static class DtoServiceRegistration
{
  public static IServiceCollection AddDtoService(this IServiceCollection services, IConfiguration configuration)
  {
    //automapper
    services.AddAutoMapper(typeof(AutoMapperConfiguration));

    //below validation registration using fluent validation lib
    services.AddSingleton<IValidator<MediaForm>, MediaFormValidator>();

    services.AddSingleton<IValidator<CityForm>, DepartmentFormValidator>();

    services.AddSingleton<IValidator<CityFilterRequest>, CityFilterRequestValidator>();

    services.AddSingleton<IValidator<GroupForm>, GroupFormValidator>();
    services.AddSingleton<IValidator<GroupFilterRequest>, GroupFilterRequestValidator>();

    services.AddSingleton<IValidator<OtpVerificationRequest>, OtpVerificationRequestValidator>();

    services.AddSingleton<IValidator<CustomerForm>, CustomerFormValidator>();

    services.AddSingleton<IValidator<UserForm>, UserFormValidator>();

    return services;
  }
}

using FluentValidation;
using KH.Dto.lookups.CityDto.Request;
using KH.Dto.lookups.CityDto.Validation;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Validation;
using KH.Dto.Lookups.CityDto.Request;
using KH.Dto.Lookups.GroupDto.Request;
using KH.Dto.Lookups.GroupDto.Validation;
using KH.Dto.MappingProfiles;
using KH.Dto.Models.AuthenticationDto.Request;
using KH.Dto.Models.AuthenticationDto.Validation;
using KH.Dto.Models.CustomerDto.Request;
using KH.Dto.Models.CustomerDto.Validation;
using KH.Dto.Models.MediaDto.Request;
using KH.Dto.Models.MediaDto.Validation;
using KH.Dto.Models.UserDto.Request;
using KH.Dto.Models.UserDto.Validation;

namespace KH.Dto;

public static class DtoServiceRegistration
{
  public static IServiceCollection AddDtoService(this IServiceCollection services, IConfiguration configuration)
  {
    //automapper
    services.AddAutoMapper(typeof(AutoMapperConfiguration));

    //below validation registration using fluent validation lib
    services.AddSingleton<IValidator<CreateMediaRequest>, MediaFormValidator>();
    services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>();

    services.AddSingleton<IValidator<CreateCityRequest>, DepartmentFormValidator>();

    services.AddSingleton<IValidator<CityFilterRequest>, CityFilterRequestValidator>();

    services.AddSingleton<IValidator<CreateGroupRequest>, GroupFormValidator>();
    services.AddSingleton<IValidator<GroupFilterRequest>, GroupFilterRequestValidator>();

    services.AddSingleton<IValidator<OtpVerificationRequest>, OtpVerificationRequestValidator>();

    services.AddSingleton<IValidator<CreateCustomerRequest>, CustomerFormValidator>();

    services.AddSingleton<IValidator<CreateUserRequest>, UserFormValidator>();
    services.AddSingleton<IValidator<List<CreateUserRequest>>, UserFormListValidator>();

    return services;
  }
}

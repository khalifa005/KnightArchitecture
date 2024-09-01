using KH.Domain.Commons;
using KH.Domain.Entities;
using KH.Dto.lookups.CityDto.Response;
using KH.Dto.Models.CalendarDto.Response;
using KH.Dto.Models.SMSDto.Form;
using KH.Dto.Models.UserDto.Response;

namespace CA.ViewModels.MappingProfiles
{
  public class DomainToDtoProfile : Profile
  {
    public DomainToDtoProfile()
    {
      CreateMap<BaseEntity, BaseEntityDto>().ReverseMap();
      CreateMap<TrackerEntity, BasicTrackerEntityDto>().ReverseMap();
      CreateMap<BasicEntity, BasicEntityWithTrackingDto>().ReverseMap();
      CreateMap<HistoryTrackerEntity, HistoryTrackerEntityDto>().ReverseMap();

      CreateMap<City, DepartmentResponse>().ReverseMap();
      //CreateMap<Role, RoleResponseDto>().ReverseMap();
      //CreateMap<Department, DepartmentResponseDto>().ReverseMap();
      CreateMap<Group, DepartmentResponse>().ReverseMap();
      CreateMap<SMSFollowUp, SMSFollowUpForm>().ReverseMap();
      CreateMap<User, UserDetailsResponse>().ReverseMap();
      CreateMap<Calendar, CalendarResponse>().ReverseMap();

    }
  }
}

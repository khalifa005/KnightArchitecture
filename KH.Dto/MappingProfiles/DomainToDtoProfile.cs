using KH.Domain.Commons;
using KH.Domain.Entities;
using KH.Dto.lookups;
using KH.Dto.lookups.Group.Response;
using KH.Dto.Models.Calendar.Response;
using KH.Dto.Models.lookups;
using KH.Dto.Models.SMS.Form;
using KH.Dto.Models.User.Response;

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

      CreateMap<City, CityResponseDto>().ReverseMap();
      CreateMap<Role, RoleResponseDto>().ReverseMap();
      CreateMap<Department, DepartmentResponseDto>().ReverseMap();
      CreateMap<Group, GroupResponse>().ReverseMap();
      CreateMap<SMSFollowUp, SMSFollowUpFormDto>().ReverseMap();
      CreateMap<User, UserDetailsResponseDto>().ReverseMap();
      CreateMap<Calendar, CalendarDetailsResponseDto>().ReverseMap();

    }
  }
}

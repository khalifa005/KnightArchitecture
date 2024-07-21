using KH.Domain.Commons;
using KH.Domain.Entities;
using KH.Dto.lookups;
using KH.Dto.Models;
using KH.Dto.Models.lookups;
using KH.Dto.Parameters.Base;

namespace CA.ViewModels.MappingProfiles
{
  public class DomainToDtoProfile : Profile
  {
    public DomainToDtoProfile()
    {
      CreateMap<BaseEntity, BaseEntityDto>().ReverseMap();
      CreateMap<TrackerEntity, BasicTrackerEntityDto>().ReverseMap();
      CreateMap<BasicEntity, BasicEntityDto>().ReverseMap();
      CreateMap<HistoryTrackerEntity, HistoryTrackerEntityDto>().ReverseMap();

      CreateMap<City, CityDto>().ReverseMap();
      CreateMap<Role, RoleDto>().ReverseMap();
      CreateMap<Department, DepartmentDto>().ReverseMap();
      CreateMap<Group, GroupDto>().ReverseMap();
      CreateMap<SMSFollowUp, SMSFollowUpDto>().ReverseMap();
      CreateMap<User, UserDto>().ReverseMap();
      CreateMap<Calendar, CalendarDto>().ReverseMap();

    }
  }
}

using KH.BuildingBlocks.Extentions.Entities;
using KH.Domain.Entities;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Response;
using KH.Dto.Models.CalendarDto.Response;
using KH.Dto.Models.SMSDto.Form;
using KH.Dto.Models.UserDto.Form;
using KH.Dto.Models.UserDto.Response;

namespace KH.Dto.MappingProfiles
{
  public class DomainToDtoProfile : Profile
  {
    public DomainToDtoProfile()
    {
      CreateMap<BaseEntity, BaseEntityDto>().ReverseMap();
      CreateMap<TrackerEntity, BasicTrackerEntityDto>().ReverseMap();
      CreateMap<BasicEntity, BasicEntityWithTrackingDto>().ReverseMap();
      CreateMap<HistoryTrackerEntity, HistoryTrackerEntityDto>().ReverseMap();

      CreateMap<User, UserForm>().ReverseMap();

      CreateMap<City, DepartmentResponse>().ReverseMap();
      //CreateMap<Role, RoleResponseDto>().ReverseMap();
      //CreateMap<Department, DepartmentResponseDto>().ReverseMap();
      CreateMap<Group, DepartmentResponse>().ReverseMap();
      CreateMap<SMSFollowUp, SMSFollowUpForm>().ReverseMap();
      CreateMap<Calendar, CalendarResponse>().ReverseMap();

      //CreateMap<User, UserDetailsResponse>().ReverseMap();

      // Mapping between User and UserDetailsResponse
      CreateMap<User, UserDetailsResponse>()
          .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.RoleId)))
          .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles))
          .ForMember(dest => dest.UserGroups, opt => opt.MapFrom(src => src.UserGroups))
          .ForMember(dest => dest.UserDepartments, opt => opt.MapFrom(src => src.UserDepartments));

      // Mapping between UserRole and UserRoleResponse
      CreateMap<UserRole, UserRoleResponse>()
          .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

      // Mapping between Role and RoleResponse (explicit property mapping)
      CreateMap<Role, RoleResponse>()
          .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
          .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
          .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
          .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedById));

      // Mapping between UserGroup and UserGroupResponse (same as Role)
      CreateMap<UserGroup, UserGroupResponse>()
          .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
          .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Group));

      // Mapping between UserDepartment and UserDepartmentResponse (same as Role)
      CreateMap<UserDepartment, UserDepartmentResponse>()
          .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
          .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department));
    }
  }
}

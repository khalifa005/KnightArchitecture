
namespace CA.ViewModels.MappingProfiles
{
    public class AutoMapperConfiguration
    {
        public AutoMapperConfiguration()
        {
            var mapConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<DomainToDtoProfile>();
            });
        }
    }
}

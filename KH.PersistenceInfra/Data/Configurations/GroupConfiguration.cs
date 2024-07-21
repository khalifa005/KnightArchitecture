

namespace KH.PersistenceInfra.Data.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.NameAr).IsRequired().HasMaxLength(300);
            builder.Property(p => p.TicketCategoryId).IsRequired(false).HasMaxLength(300).HasColumnOrder(5);

        }
    }


}

namespace KH.PersistenceInfra.Data.Configurations;

public class CalenderConfiguration : IEntityTypeConfiguration<Calendar>
{
  public void Configure(EntityTypeBuilder<Calendar> builder)
  {
    //additional configuration for product table
    builder.Property(p => p.Id).UseIdentityColumn();


  }
}

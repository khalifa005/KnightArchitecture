

namespace KH.PersistenceInfra.Data.Configurations;

public class EmailTrackerConfiguration : IEntityTypeConfiguration<EmailTracker>
{
  public void Configure(EntityTypeBuilder<EmailTracker> builder)
  {
    builder.Property(p => p.Id).UseIdentityColumn().HasColumnOrder(1);

    builder.Property(p => p.Model).IsRequired().HasMaxLength(300).HasColumnOrder(2);
    builder.Property(p => p.ModelId).IsRequired().HasMaxLength(300).HasColumnOrder(3);
    builder.Property(p => p.MailType).IsRequired().HasMaxLength(300).HasColumnOrder(4);

  }
}

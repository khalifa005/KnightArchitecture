
namespace KH.PersistenceInfra.Data.Configurations;

public abstract class SmsTrackerConfiguration : IEntityTypeConfiguration<SmsTracker>
{
  public virtual void Configure(EntityTypeBuilder<SmsTracker> builder)
  {
    builder.Property(m => m.MobileNumber).IsRequired().HasColumnOrder(2);
    builder.Property(m => m.Message).IsRequired().HasColumnOrder(3);
    builder.Property(m => m.ModelId).IsRequired().HasColumnOrder(4);
    builder.Property(m => m.IsSent).HasColumnOrder(5);
    builder.Property(m => m.FailureReasons).IsRequired().HasColumnOrder(6);

  }
}

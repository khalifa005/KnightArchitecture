
namespace KH.PersistenceInfra.Data.Configurations;

public abstract class SMSFollowUpConfiguration : IEntityTypeConfiguration<SMSFollowUp>
{
  public virtual void Configure(EntityTypeBuilder<SMSFollowUp> builder)
  {
    builder.Property(m => m.MobileNumber).IsRequired().HasColumnOrder(2);
    builder.Property(m => m.Message).IsRequired().HasColumnOrder(3);
    builder.Property(m => m.ModelId).IsRequired().HasColumnOrder(4);
    builder.Property(m => m.IsSent).HasColumnOrder(5);
    builder.Property(m => m.FailReason).IsRequired().HasColumnOrder(6);



  }
}

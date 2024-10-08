
namespace KH.PersistenceInfra.Data.Configurations;

public abstract class SmsTemplateConfiguration : IEntityTypeConfiguration<SmsTemplate>
{
  public virtual void Configure(EntityTypeBuilder<SmsTemplate> builder)
  {
    builder.Property(m => m.Id).UseIdentityColumn().HasColumnOrder(1);
    builder.Property(m => m.SmsType).IsRequired().HasColumnOrder(3);
    builder.Property(m => m.TextAr).IsRequired().HasColumnOrder(4);
    builder.Property(m => m.TextEn).HasColumnOrder(5);

  }
}

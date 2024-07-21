
namespace KH.PersistenceInfra.Data.Configurations
{
  public abstract class MediaConfiguration : IEntityTypeConfiguration<Media>
  {
    public virtual void Configure(EntityTypeBuilder<Media> builder)
    {
      builder.Property(m => m.ModelId).IsRequired().HasColumnOrder(2);
      builder.Property(m => m.FileName).IsRequired().HasColumnOrder(3);
      builder.Property(m => m.Path).IsRequired().HasColumnOrder(4);
      builder.Property(m => m.ContentType).HasColumnOrder(5);
      builder.Property(m => m.Extention).HasColumnOrder(6);
      builder.Property(m => m.Model).IsRequired().HasColumnOrder(7);


    }
  }
}

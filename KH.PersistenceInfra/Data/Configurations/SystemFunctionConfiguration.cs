
namespace KH.PersistenceInfra.Data.Configurations;

public class SystemFunctionConfiguration : IEntityTypeConfiguration<SystemActions>
{
  public void Configure(EntityTypeBuilder<SystemActions> builder)
  {
    builder.Property(p => p.Id).ValueGeneratedNever();
    builder.Property(p => p.NameAr).IsRequired();
    builder.Property(p => p.ParentID).IsRequired(false).HasColumnOrder(5);
    builder.Property(p => p.SortKey).HasColumnOrder(6);
    builder.Property(p => p.DependOnID).IsRequired(false).HasColumnOrder(7);

    builder.HasOne(x => x.Parent)
              .WithMany()
              .HasForeignKey(x => x.ParentID);



  }
}

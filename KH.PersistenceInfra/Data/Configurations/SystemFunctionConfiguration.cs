
namespace KH.PersistenceInfra.Data.Configurations;

public class SystemFunctionConfiguration : IEntityTypeConfiguration<Permission>
{
  public void Configure(EntityTypeBuilder<Permission> builder)
  {
    builder.Property(p => p.Id).ValueGeneratedNever();
    builder.Property(p => p.NameAr).IsRequired();
    builder.Property(p => p.ParentId).IsRequired(false).HasColumnOrder(5);
    builder.Property(p => p.SortKey).HasColumnOrder(6);
    builder.Property(p => p.DependOnId).IsRequired(false).HasColumnOrder(7);

    
    // Self-referencing relationship for Parent Permission
    builder.HasOne(p => p.Parent)
           .WithMany(p => p.Children) // Define inverse relationship
           .HasForeignKey(p => p.ParentId)
           .OnDelete(DeleteBehavior.Restrict); // Optional: set delete behavior



  }
}

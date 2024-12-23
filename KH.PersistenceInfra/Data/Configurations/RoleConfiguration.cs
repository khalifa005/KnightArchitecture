using Role = KH.Domain.Entities.Role;
namespace KH.PersistenceInfra.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    //additional configuration for product table
    builder.Property(p => p.Id).ValueGeneratedNever();

    builder.Property(u => u.RowVersion)
      .IsRowVersion();


    builder.Property(p => p.NameAr).IsRequired().HasMaxLength(300);
    builder.Property(p => p.ReportToRoleId).IsRequired(false).HasMaxLength(300).HasColumnOrder(5);

    builder.HasOne(t => t.ReportToRole)
        .WithMany()
          .HasForeignKey(t => t.ReportToRoleId);

    // Define self-referencing relationship with explicit collection
    builder.HasOne(t => t.ReportToRole)
        .WithMany(r => r.SubRoles) // Add a collection to the Role class
        .HasForeignKey(t => t.ReportToRoleId)
        .OnDelete(DeleteBehavior.Restrict);

    // RolePermissions relationship
    builder.HasMany(t => t.RolePermissions)
        .WithOne(c => c.Role)
        .HasForeignKey(c => c.RoleId)
        .OnDelete(DeleteBehavior.Restrict); // Define delete behavior


  }
}

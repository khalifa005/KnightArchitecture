namespace KH.PersistenceInfra.Data.Configurations;

public class RoleFunctionConfiguration : IEntityTypeConfiguration<RolePermissions>
{
  public void Configure(EntityTypeBuilder<RolePermissions> builder)
  {
    builder.Property(p => p.Id).UseIdentityColumn();
    builder.Property(p => p.PermissionId).HasColumnOrder(2);
    builder.Property(p => p.RoleId).HasColumnOrder(3);


    // Define relationship with Permission
    builder.HasOne(rp => rp.Permission)
           .WithMany(p => p.RolePermissions) // Explicitly define inverse relationship
           .HasForeignKey(rp => rp.PermissionId)
           .OnDelete(DeleteBehavior.Restrict);

    // Define relationship with Role
    builder.HasOne(rp => rp.Role)
           .WithMany(r => r.RolePermissions) // Explicitly define inverse relationship
           .HasForeignKey(rp => rp.RoleId)
           .OnDelete(DeleteBehavior.Restrict);
  }
}

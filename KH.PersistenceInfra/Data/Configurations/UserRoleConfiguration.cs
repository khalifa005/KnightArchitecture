
namespace KH.PersistenceInfra.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    // Define relationship with Permission
    builder.HasOne(rp => rp.User)
           .WithMany(p => p.UserRoles) // Explicitly define inverse relationship
           .HasForeignKey(rp => rp.UserId)
           .OnDelete(DeleteBehavior.Restrict);

    // Define relationship with Role
    builder.HasOne(rp => rp.Role)
           .WithMany() // Explicitly define inverse relationship
           .HasForeignKey(rp => rp.RoleId)
           .OnDelete(DeleteBehavior.Restrict);

  }
}

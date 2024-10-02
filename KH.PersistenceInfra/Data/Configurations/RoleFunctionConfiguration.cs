namespace KH.PersistenceInfra.Data.Configurations;

public class RoleFunctionConfiguration : IEntityTypeConfiguration<RolePermissions>
{
  public void Configure(EntityTypeBuilder<RolePermissions> builder)
  {
    builder.Property(p => p.Id).UseIdentityColumn();
    builder.Property(p => p.SystemActionsId).HasColumnOrder(2);
    builder.Property(p => p.RoleId).HasColumnOrder(3);

    builder.HasOne(t => t.SystemActions)
              .WithMany()
                .HasForeignKey(t => t.SystemActionsId);

    builder.HasOne(t => t.Role)
        .WithMany()
          .HasForeignKey(t => t.RoleId);


  }
}

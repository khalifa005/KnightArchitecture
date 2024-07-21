namespace KH.PersistenceInfra.Data.Configurations
{
  public class RoleFunctionConfiguration : IEntityTypeConfiguration<RoleFunction>
  {
    public void Configure(EntityTypeBuilder<RoleFunction> builder)
    {
      builder.Property(p => p.Id).UseIdentityColumn();
      builder.Property(p => p.SystemFunctionId).HasColumnOrder(2);
      builder.Property(p => p.RoleId).HasColumnOrder(3);

      builder.HasOne(t => t.SystemFunction)
                .WithMany()
                  .HasForeignKey(t => t.SystemFunctionId);

      builder.HasOne(t => t.Role)
          .WithMany()
            .HasForeignKey(t => t.RoleId);


    }
  }


}

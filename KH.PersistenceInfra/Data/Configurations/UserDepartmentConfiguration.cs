namespace KH.PersistenceInfra.Data.Configurations;

public class UserDepartmentConfiguration : IEntityTypeConfiguration<UserDepartment>
{
  public void Configure(EntityTypeBuilder<UserDepartment> builder)
  {
    builder.Property(p => p.Id).UseIdentityColumn();
    builder.Property(p => p.DepartmentId).HasColumnOrder(2);
    builder.Property(p => p.UserId).HasColumnOrder(3);

    builder.HasOne(t => t.User)
        .WithMany(x => x.UserDepartments)
          .HasForeignKey(t => t.UserId);

    builder.HasOne(t => t.Department)
        .WithMany()
          .HasForeignKey(t => t.DepartmentId);


  }
}

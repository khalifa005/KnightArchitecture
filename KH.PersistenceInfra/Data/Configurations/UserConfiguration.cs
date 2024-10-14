namespace KH.PersistenceInfra.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    //additional configuration for product table
    builder.Property(p => p.Id).UseIdentityColumn();


    builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100).HasColumnOrder(2);
    builder.Property(p => p.MiddleName).IsRequired().HasMaxLength(100).HasColumnOrder(3);
    builder.Property(p => p.LastName).IsRequired().HasMaxLength(100).HasColumnOrder(4);
    builder.Property(p => p.MobileNumber).HasColumnOrder(5);
    builder.Property(p => p.Email).HasColumnOrder(6);
    builder.Property(p => p.Username).HasColumnOrder(7);
    builder.Property(p => p.BirthDate).HasColumnOrder(8);

    //builder.Property(p => p.RowVersion)
    //  .IsRowVersion(); // EF will use this column for concurrency checks;



    builder.HasMany(t => t.UserRoles)
           .WithOne(c => c.User)
           .HasForeignKey(t => t.UserId);

    builder.HasMany(t => t.UserDepartments)
           .WithOne(c => c.User)
           .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete;

  }
}

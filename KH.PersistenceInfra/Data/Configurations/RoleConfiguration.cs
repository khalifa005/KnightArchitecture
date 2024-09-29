using Role = KH.Domain.Entities.Role;
namespace KH.PersistenceInfra.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    //additional configuration for product table
    builder.Property(p => p.Id).ValueGeneratedNever();


    builder.Property(p => p.NameAr).IsRequired().HasMaxLength(300);
    builder.Property(p => p.ReportToRoleId).IsRequired(false).HasMaxLength(300).HasColumnOrder(5);

    builder.HasOne(t => t.ReportToRole)
        .WithMany()
          .HasForeignKey(t => t.ReportToRoleId);



  }
}

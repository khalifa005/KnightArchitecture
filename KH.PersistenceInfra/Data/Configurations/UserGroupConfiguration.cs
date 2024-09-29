namespace KH.PersistenceInfra.Data.Configurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
  public void Configure(EntityTypeBuilder<UserGroup> builder)
  {
    builder.Property(p => p.Id).UseIdentityColumn();
    builder.Property(p => p.GroupId).HasColumnOrder(2);
    builder.Property(p => p.UserId).HasColumnOrder(3);

    builder.HasOne(t => t.User)
              .WithMany(x => x.UserGroups)
                .HasForeignKey(t => t.UserId);

    builder.HasOne(t => t.Group)
        .WithMany()
          .HasForeignKey(t => t.GroupId);


  }
}

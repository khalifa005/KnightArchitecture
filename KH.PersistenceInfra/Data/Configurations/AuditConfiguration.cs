

using KH.BuildingBlocks.Apis.Entities;

namespace KH.PersistenceInfra.Data.Configurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
  public void Configure(EntityTypeBuilder<Audit> builder)
  {
    builder.Property(p => p.Id).UseIdentityColumn();

    //additional configuration for product table
    builder
      .HasIndex(e => e.UserId); // Defining the index using Fluent API

    //.HasDatabaseName("IX_MyEntity_MyStringField"); // Optional: specify a custom index name

  }
}

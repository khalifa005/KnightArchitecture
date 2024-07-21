namespace KH.PersistenceInfra.Data.Configurations
{
  public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
  {
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
      //additional configuration for product table
      builder.Property(p => p.Id).UseIdentityColumn().HasColumnOrder(1);
      builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100).HasColumnOrder(2);
      builder.Property(p => p.MiddleName).IsRequired().HasMaxLength(100).HasColumnOrder(3);
      builder.Property(p => p.LastName).IsRequired().HasMaxLength(100).HasColumnOrder(4);
      builder.Property(p => p.Email).HasColumnOrder(5);
      builder.Property(p => p.Username).HasColumnOrder(6);
      builder.Property(p => p.Password).HasColumnOrder(7);
      builder.Property(p => p.BirthDate).HasColumnOrder(8);
      builder.Property(p => p.MobileNumber).IsRequired().HasColumnOrder(9);
      builder.Property(p => p.IDType).IsRequired().HasColumnOrder(10);
      builder.Property(p => p.IDNumber).HasMaxLength(10).HasColumnOrder(11);
      builder.Property(p => p.IsSelfRegistered).HasDefaultValue(false).HasColumnOrder(12);
      builder.Property(p => p.OTPCode).HasMaxLength(10).HasColumnOrder(13);
      builder.Property(p => p.IsOTPVerified).HasDefaultValue(false).HasColumnOrder(14);
      builder.Property(p => p.IsForgetPasswordOTPVerified).HasDefaultValue(false).HasColumnOrder(15);



    }
  }



}

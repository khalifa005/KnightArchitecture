using KH.BuildingBlocks.Enums;
using Role = KH.Domain.Entities.Role;

namespace KH.PersistenceInfra.Data.Seed;

public class SmsTemplateContextSeeder
{
  private static readonly DateTime SeedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  public static void SeedTemplates(ModelBuilder builder, ILoggerFactory loggerFactory)
  {
    try
    {
      var entities = new List<SmsTemplate>()
      {
        new SmsTemplate
            {
                Id = 1, // Example ID
                SmsType = SmsTypeEnum.WelcomeUser.ToString(), // Assuming you have an enum SmsTypeEnum
                TextEn = "Welcome {FirstName}, your OTP code is {OtpCode}.", // English template
                TextAr = "مرحبًا {FirstName}، رمز التحقق الخاص بك هو {OtpCode}.", // Arabic template
                CreatedDate = SeedDate,
            },
            new SmsTemplate
            {
                Id = 2,
                SmsType = SmsTypeEnum.OTP.ToString(), // Another type of SMS template
                TextEn = "Hello {Username}, use this OTP {OtpCode} to reset your password.",
                TextAr = "مرحبًا {Username}، استخدم هذا الرمز {OtpCode} لإعادة تعيين كلمة المرور.",
                CreatedDate = SeedDate,
            }
      };

      builder.Entity<SmsTemplate>().HasData(entities);

    }
    catch (Exception ex)
    {
      var logger = loggerFactory.CreateLogger<SmsTemplateContextSeeder>();
      logger.LogError(ex.Message);
    }
  }
  
}

using KH.Dto.Models.EmailDto.Request;
using KH.Dto.Models.SMSDto.Request;
using KH.Services.Emails.Contracts;
using KH.Services.Sms.Contracts;

namespace KH.Services.Users.Implementation;

public class UserNotificationService : IUserNotificationService
{
  private readonly ICurrentUserService _currentUserService;
  private readonly ISmsService _smsService;
  private readonly ISmsTemplateService _smsTemplateService;
  private readonly IEmailService _emailService;
  private readonly ILogger<UserNotificationService> _logger;
  public UserNotificationService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ITokenService tokenService,
    ISmsService smsService,
    ISmsTemplateService smsTemplateService,
    IEmailService emailService,
    IMapper mapper,
    IUserValidationService userValidationService,
    IHttpContextAccessor httpContextAccessor,
    IHostEnvironment env,
    ILogger<UserNotificationService> logger)
  {
    _currentUserService = currentUserService;
    _smsService = smsService;
    _smsTemplateService = smsTemplateService;
    _emailService = emailService;
    _logger = logger;
  }

  public async Task<ApiResponse<string>> SendUserWelcomeSmsAsync(User userEntity, CancellationToken cancellationToken)
  {
    //ApiResponse<string> res = new ApiResponse<string>((int)HttpStatusCode.OK);

    var smsWelcomeTemplateResult = await _smsTemplateService.GetSmsTemplateAsync(SmsTypeEnum.WelcomeUser.ToString(), cancellationToken);
    if (smsWelcomeTemplateResult.StatusCode != StatusCodes.Status200OK || smsWelcomeTemplateResult.Data == null)
      return new ApiResponse<string>((int)HttpStatusCode.BadRequest);

    var templateContent = _smsTemplateService.GetTemplateForLanguage(smsWelcomeTemplateResult.Data, LanguageEnum.English);
    var formattedMessage = _smsTemplateService.ReplaceWelcomeSmsPlaceholders(templateContent, userEntity);

    var smsTrackerForm = new CreateSmsTrackerRequest
    {
      MobileNumber = userEntity.MobileNumber,
      Message = formattedMessage,
      Model = ModelEnum.User.ToString(),
      ModelId = userEntity.Id
    };

    var result = await _smsService.SendSmsAsync(smsTrackerForm, cancellationToken);
    return result;
  }
  public async Task<ApiResponse<string>> SendUserWelcomeEmailAsync(User userEntity, CancellationToken cancellationToken)
  {
    var mailRequest = new MailRequest
    {
      Subject = MailTypeEnum.WelcomeTemplate.ToString(),
      Model = ModelEnum.User.ToString(),
      ModelId = userEntity.Id,
      MailType = MailTypeEnum.WelcomeTemplate,
      ToEmail = new List<string?> { userEntity.Email },
      PrefaredLanguageKey = "En"
    };
    var res = await _emailService.SendEmailAsync(mailRequest, cancellationToken);

    return res;
  }

}

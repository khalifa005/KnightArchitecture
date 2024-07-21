namespace KH.Helper.Settings
{
  public class MailSettings
  {
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Host { get; set; }
    public string? AdminTicketingHostUrl { get; set; }
    public int Port { get; set; }
    public bool? IsActive { get; set; }
  }
}

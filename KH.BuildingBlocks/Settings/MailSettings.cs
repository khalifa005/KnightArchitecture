namespace KH.BuildingBlocks.Settings;

public class MailSettings
{
  public string Mail { get; set; }
  public string DisplayName { get; set; }
  public string Password { get; set; }
  public string Host { get; set; }
  public string? AdminTicketingHostUrl { get; set; }
  public int Port { get; set; }
  public bool Disable { get; set; }
}

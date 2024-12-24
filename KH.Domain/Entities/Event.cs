namespace KH.Domain.Entities;

public class Event
{
  public int EventId { get; set; }
  public DateTime Timestamp { get; set; }
  public string EventType { get; set; }
  public string Payload { get; set; }
}
public class Subscription
{
  public int SubscriptionId { get; set; }
  public string SubscriberId { get; set; }
  public string EventType { get; set; }
  public string CallbackUrl { get; set; }
  public string Secret { get; set; }
}

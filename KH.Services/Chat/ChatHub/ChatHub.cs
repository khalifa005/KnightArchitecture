using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.Chat.ChatHub;
public class ChatHub : Hub<IChatHub>
{
  public async Task BroadcastAsync(ChatMessage message)
  {
    await Clients.All.MessageReceivedFromHub(message);
  }
  public override async Task OnConnectedAsync()
  {
    await Clients.All.NewUserConnected("a new user connectd");
  }
}

public interface IChatHub
{
  Task MessageReceivedFromHub(ChatMessage message);

  Task NewUserConnected(string message);
}

public class ChatMessage
{
  public string Text { get; set; }
  public string ConnectionId { get; set; }
  public DateTime DateTime { get; set; }
}


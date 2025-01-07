using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.Chat.ChatHub;


using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

  public class CChatHub : Hub
  {
    private static readonly ConcurrentDictionary<string, ChatUser> Users = new();

    public async Task SendMessage(string message, string group, string name)
    {
      if (string.IsNullOrEmpty(group))
        group = "DefaultGroup";

      if (!Users.TryGetValue(Context.ConnectionId, out ChatUser user))
      {
        user = JoinGroup(name, group);
      }

      user.LastOn = DateTime.UtcNow;

      var msg = new ChatMessagess { Message = message, User = user, IsCurrentUser = true, Time = DateTime.UtcNow, };
      await Clients.Group(group).SendAsync("OnReceiveMessage", msg);

  }

    public ChatUser JoinGroup(string name, string groupName)
    {
      Groups.AddToGroupAsync(Context.ConnectionId, groupName);
      var user = new ChatUser { Name = name, Group = groupName, Id = Context.ConnectionId };
      Users[Context.ConnectionId] = user;
      return user;
    }



  public async Task LeaveGroup(string groupName)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
      Users.TryRemove(Context.ConnectionId, out _);
    }


  public override Task OnDisconnectedAsync(Exception? exception)
  {
    Users.TryRemove(Context.ConnectionId, out _);
    return base.OnDisconnectedAsync(exception);
  }

}

  public class ChatUser
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime LastOn { get; set; }
  }

  public class ChatMessagess
  {
    public ChatUser User { get; set; }
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public bool IsCurrentUser { get; set; }
  }

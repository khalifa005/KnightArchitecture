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
  // In-memory storage for groups and their messages
  private static readonly ConcurrentDictionary<string, List<ChatMessagess>> GroupMessages = new();
  private static readonly ConcurrentDictionary<string, ChatUser> Users = new();

  public async Task SendMessage(string message, string group, string userName)
  {
    if (string.IsNullOrEmpty(group)) return;

    var chatMessage = new ChatMessagess
    {
      User = new ChatUser { Name = userName, Group = group },
      Message = message,
      Time = DateTime.UtcNow
    };

    // Save message in the group
    if (!GroupMessages.ContainsKey(group))
    {
      GroupMessages[group] = new List<ChatMessagess>();
    }
    GroupMessages[group].Add(chatMessage);

    // Send message to all clients in the group
    await Clients.Group(group).SendAsync("OnReceiveMessage", chatMessage);
  }

  public async Task JoinGroup(string userName, string group)
  {
    if (string.IsNullOrEmpty(group)) return;

    // Add user to the group
    await Groups.AddToGroupAsync(Context.ConnectionId, group);

    if (!Users.ContainsKey(Context.ConnectionId))
    {
      Users[Context.ConnectionId] = new ChatUser { Name = userName, Group = group };
    }

    // Notify others in the group
    var joinMessage = new ChatMessagess
    {
      User = new ChatUser { Name = "System", Group = group },
      Message = $"{userName} has joined the group.",
      Time = DateTime.UtcNow
    };

    await Clients.Group(group).SendAsync("OnReceiveMessage", joinMessage);
  }

  public async Task ExitGroup(string group)
  {
    if (string.IsNullOrEmpty(group)) return;

    // Remove user from the group
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);

    if (Users.ContainsKey(Context.ConnectionId))
    {
      var user = Users[Context.ConnectionId];
      if (user.Group == group)
      {
        Users.Remove(Context.ConnectionId, out _);
      }
    }

    // Notify others in the group
    var exitMessage = new ChatMessagess
    {
      User = new ChatUser { Name = "System", Group = group },
      Message = $"{Context.ConnectionId} has left the group.",
      Time = DateTime.UtcNow
    };

    await Clients.Group(group).SendAsync("OnReceiveMessage", exitMessage);
  }

  public async Task<List<ChatMessagess>> GetGroupMessages(string group)
  {
    // Return all messages for the group
    if (GroupMessages.ContainsKey(group))
    {
      return GroupMessages[group];
    }

    return new List<ChatMessagess>();
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

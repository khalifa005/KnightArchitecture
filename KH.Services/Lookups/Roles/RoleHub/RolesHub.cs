using AngleSharp.Dom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using static QuestPDF.Helpers.Colors;

namespace KH.Services.Lookups.Roles.RoleHub;

//[Authorize]
public class RolesHub : Hub
{
  //: Hub<IRoleClient>
  // You can define methods here if needed
  //define functions on the hub that your client can invoke,
  //but your server can also invoke functions on the client.
  public async Task EmitActiveRole()
  {
    //var orders = _context.Orders.Include(x => x.FoodItem).Where(x => x.OrderState != OrderState.Completed).ToList();

    //await Clients.All.PendingRoleUpdated(role);
  }

  public override async Task OnConnectedAsync()
  {
    Console.WriteLine(Context.ConnectionId);
    await base.OnConnectedAsync();
  }

  public override async Task OnDisconnectedAsync(Exception ex)
  {
    Console.WriteLine(Context.ConnectionId);
    await base.OnDisconnectedAsync(ex);
  }
}

// These are the RPC calls on the client
public interface IRoleClient
{
  Task PendingRoleUpdated(Domain.Entities.Role role);
}

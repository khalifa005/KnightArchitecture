using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;
using KH.Services.Chat.ChatHub;
using KH.Services.Lookups.Departments.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace KH.WebApi.Controllers;

public class ChatController : BaseApiController
{
  public readonly IDepartmentService _lookupService;

  private readonly IHubContext<ChatHub> _hubContext;

  public ChatController(IDepartmentService lookupService, IHubContext<ChatHub> hubContext)
  {
    _lookupService = lookupService;
    _hubContext = hubContext;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<DepartmentResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }



  [HttpPost]
  public async Task SendMessage(ChatMessage message)
  {
    //additional business logic 

    await this._hubContext.Clients.All.SendAsync("messageReceivedFromApi", message);

    //additional business logic 
  }


}


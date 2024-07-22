using KH.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
  private readonly UserService _userService;

  public UserController(UserService userService)
  {
    _userService = userService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateUser([FromBody] User user)
  {
    await _userService.AddUserAsync(user);
    return Ok();
  }

  [HttpPost("bulk")]
  public async Task<IActionResult> CreateUsers([FromBody] IEnumerable<User> users)
  {
    await _userService.AddUsersAsync(users);
    return Ok();
  }

  [HttpGet]
  public async Task<IActionResult> GetUsers(
      [FromQuery] string firstName = null,
      [FromQuery] string lastName = null,
      [FromQuery] string email = null,
      [FromQuery] string mobileNumber = null)
  {
    var users = await _userService.GetAllUsersWithIncludesAsync(firstName, lastName, email, mobileNumber);
    return Ok(users);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetUser(long id)
  {
    var user = await _userService.GetUserByIdAsync(id);
    if (user == null) return NotFound();
    return Ok(user);
  }

  [HttpGet("{id}/with-includes")]
  public async Task<IActionResult> GetUserWithIncludes(long id)
  {
    var user = await _userService.GetUserByIdWithIncludesAsync(id);
    if (user == null) return NotFound();
    return Ok(user);
  }

  [HttpGet("find")]
  public IActionResult FindUsers([FromQuery] string search)
  {
    var users = _userService.FindUsers(u => u.FirstName.Contains(search) || u.LastName.Contains(search));
    return Ok(users);
  }

  [HttpGet("find-with-includes")]
  public async Task<IActionResult> FindUsersWithIncludes([FromQuery] string search)
  {
    var users = await _userService.FindUsersWithIncludesAsync(u => u.FirstName.Contains(search) || u.LastName.Contains(search));
    return Ok(users);
  }

  [HttpGet("count")]
  public async Task<IActionResult> CountUsers()
  {
    var count = await _userService.CountUsersAsync();
    return Ok(count);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateUser([FromBody] User user)
  {
    await _userService.UpdateUserAsync(user);
    return Ok();
  }

  [HttpPut("bulk")]
  public async Task<IActionResult> UpdateUsers([FromBody] IEnumerable<User> users)
  {
    await _userService.UpdateUsersAsync(users);
    return Ok();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteUser(long id)
  {
    await _userService.DeleteUserAsync(id);
    return Ok();
  }
}

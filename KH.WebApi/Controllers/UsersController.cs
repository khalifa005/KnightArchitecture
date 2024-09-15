using Azure.Core;
using CA.Services.Contracts;
using KH.Dto.Models.UserDto.Form;
using KH.Dto.Models.UserDto.Response;
using KH.Helper.Extentions;
using KH.Helper.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KH.WebApi.Controllers
{
  //[Route("api/[controller]")]
  //[ApiController]
  public class UsersController : BaseApiController
  {
    public readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
      _userService = userService;
    }
    // GET: api/<UsersController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> Get(int id)
    {
      var res = await _userService.GetAsync(id);
      return AsActionResult(res);
    }

    // POST api/<UsersController>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] UserForm request)
    {
      var res = await _userService.AddAsync(request);
      return AsActionResult(res);
    }

    [HttpPost("AddRange")]
    public async Task<ActionResult<ApiResponse<string>>> PostRange([FromBody] List<UserForm> request)
    {
      var res = await _userService.AddListAsync(request);
      return AsActionResult(res);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] UserForm request)
    {
      var res = await _userService.UpdateAsync(request);
      return AsActionResult(res);
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}

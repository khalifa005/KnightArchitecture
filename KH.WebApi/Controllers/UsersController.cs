using CA.Services.Contracts;
using KH.Dto.Models.UserDto.Form;
using KH.Helper.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KH.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
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
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<UsersController>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] UserForm request)
    {
      var res = await _userService.RegisterUserAsync(request);
      return Ok(res);
      //return AsActionResult(item); use later in cuustom base api
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}

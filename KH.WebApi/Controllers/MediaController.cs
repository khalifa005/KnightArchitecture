using KH.Helper.Extentions;
using Microsoft.AspNetCore.Mvc;


namespace KH.WebApi.Controllers
{
  public class MediaController : BaseApiController
  {
    // GET: api/<MediaController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/<MediaController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<MediaController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<MediaController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<MediaController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}

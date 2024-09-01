using KH.Helper.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KH.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DemoController : ControllerBase
  {


    // GET: api/<DemoController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/<DemoController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<DemoController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<DemoController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<DemoController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }


    [HttpPost("SubmitFormlyForm")]
    public async Task<IActionResult> SubmitFormUsingBase64([FromBody] JObject form)
    {
      var fileBase64 = form["file"]?.ToString();


      if (!string.IsNullOrEmpty(fileBase64))
      {
        try
        {
          var base64Data = fileBase64.Split(",")[1]; // Remove the data:image/...;base64, part
          var fileData = Convert.FromBase64String(base64Data);
          var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "uploadedFile.png"); // Generate a unique filename if necessary

          // Ensure the uploads directory exists
          Directory.CreateDirectory(Path.GetDirectoryName(filePath));

          await System.IO.File.WriteAllBytesAsync(filePath, fileData);
        }
        catch (FormatException ex)
        {
          return BadRequest("Invalid base64 string format.");
        }
      }


      return Ok(new { Message = "Form submitted successfully!" });
    }

    [HttpPost("SubmitFormlyFormFileStream")]
    public async Task<IActionResult> SubmitFormUsingFileStream([FromForm] IFormCollection form)
    {

      var files = Request.Form.Files;

      var fileGroups = files.GroupBy(file => file.Name.Split('[')[0]); // Group files by key prefix

      foreach (var group in fileGroups)
      {
        string key = group.Key;
        foreach (var file in group)
        {
          if (file.Length > 0)
          {
            // Process each file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }
          }
        }
      }

      // Handle file uploads
      foreach (var file in form.Files)
      {
        if (file.Length > 0)
        {
          // Generate a unique filename to avoid overwriting existing files
          var fileName = Path.GetFileName(file.FileName);
          var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(fileName);

          // Ensure the uploads directory exists
          var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
          Directory.CreateDirectory(uploadsDir);

          // Combine the directory path and the unique filename
          var filePath = Path.Combine(uploadsDir, uniqueFileName);

          // Save the file to the server
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }

          System.Diagnostics.Debug.WriteLine($"File saved: {filePath}");
          return Ok(new FileResponse { FilePath = $"{filePath}", FileId = $"{filePath}" });

        }
      }


      return Ok(new FileResponse { FilePath = $"Form-submitted-successfully!" });
    }


    [HttpGet("DeleteFile")]
    public async Task<IActionResult> DeleteFile(string ID, string FBFID)
    {


      return Ok(new FileResponse { FilePath = $"Deleted-successfully!" });
    }



  }
}





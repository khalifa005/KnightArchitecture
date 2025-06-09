using KH.BuildingBlocks.Apis.Extentions;
using KH.Services.Speech.SpeechHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace KH.WebApi.Controllers;

public class SpeechController : BaseApiController
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<SpeechHub> _hubContext;

    public SpeechController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHubContext<SpeechHub> hubContext)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _hubContext = hubContext;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAudio(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var transcript = await SendToWhisperAsync(ms);
        await _hubContext.Clients.All.SendAsync("TranscriptionResult", transcript);

        var audioUrl = await SendToTtsAsync(string.Join(" ", transcript.Values));
        await _hubContext.Clients.All.SendAsync("TtsReady", audioUrl);
        return Ok();
    }

    private async Task<Dictionary<string, string>> SendToWhisperAsync(Stream audio)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(audio), "file", "audio.webm");
        content.Add(new StringContent("whisper-1"), "model");
        var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", content);
        var result = await response.Content.ReadAsStringAsync();
        // expected result { "text": "..." }
        var text = System.Text.Json.JsonDocument.Parse(result).RootElement.GetProperty("text").GetString();
        // naive mapping for street city state zip separated by commas
        var parts = text?.Split(',') ?? Array.Empty<string>();
        var dict = new Dictionary<string, string>();
        if (parts.Length > 0) dict["street"] = parts[0].Trim();
        if (parts.Length > 1) dict["city"] = parts[1].Trim();
        if (parts.Length > 2) dict["state"] = parts[2].Trim();
        if (parts.Length > 3) dict["zip"] = parts[3].Trim();
        return dict;
    }

    private async Task<string> SendToTtsAsync(string text)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { input = text, model = "tts-1" }), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);
        var bytes = await response.Content.ReadAsByteArrayAsync();
        var fileName = Path.GetRandomFileName() + ".mp3";
        var path = Path.Combine("wwwroot", fileName);
        await System.IO.File.WriteAllBytesAsync(path, bytes);
        return $"/" + fileName;
    }
}

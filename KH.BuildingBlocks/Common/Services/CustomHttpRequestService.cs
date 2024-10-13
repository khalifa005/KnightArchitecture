using KH.BuildingBlocks.Apis.Responses;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Mime;

namespace KH.BuildingBlocks.Apis.Services;

public class CustomHttpRequestService
{
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly ILogger<CustomHttpRequestService> _logger;

  public CustomHttpRequestService(IHttpClientFactory httpClientFactory, ILogger<CustomHttpRequestService> logger)
  {
    _httpClientFactory = httpClientFactory;
    _logger = logger;
  }

  public async Task<HeroResult<T>> GetRequestAsync<T>(string Endpoint, string token, bool returnedAsArrayByte = false)
  {
    HttpRequestMessage httpRequestMessage = new();

    httpRequestMessage.Method = new HttpMethod(HttpMethod.Get.ToString());

    httpRequestMessage.RequestUri = new Uri(Endpoint);

    //httpRequestMessage.Content.Headers.ContentType
    //    = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeHeaderTypes.ApplicationJson);
    if (!string.IsNullOrEmpty(token))
    {
      httpRequestMessage.Headers.Authorization
         = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    var client = _httpClientFactory.CreateClient();
    var result = await client.SendAsync(httpRequestMessage);

    dynamic content = null;

    if (returnedAsArrayByte)
      content = await result.Content.ReadAsByteArrayAsync();
    else
      content = await result.Content.ReadAsStringAsync();


    _logger.Log(LogLevel.Information, $"calling {Endpoint} status code {result.StatusCode} response as string : {content}");

    if (result.StatusCode == HttpStatusCode.InternalServerError ||
       result.StatusCode == HttpStatusCode.BadRequest ||
       result.StatusCode == HttpStatusCode.Unauthorized)
    {
      _logger.Log(LogLevel.Error, "httprequest error occurred while calling post Endpoint: {@Endpoint}  response  : {@result}", Endpoint, result);
      Console.WriteLine(content);
      return HeroResult<T>.False(content);
    }

    //if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    //{
    //    return Result<T>.False(content);
    //}
    //else if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
    //{
    //    System.Console.WriteLine(content);
    //    return Result<T>.False(content);
    //}
    //else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
    //{
    //    System.Console.WriteLine(content);
    //    return Result<T>.False(content);
    //}

    try
    {
      dynamic res = null;
      if (returnedAsArrayByte)
        res = content;
      else
        res = JsonConvert.DeserializeObject<T>(content);

      return HeroResult<T>.True(res);
    }
    catch (Exception e)
    {
      _logger.Log(LogLevel.Error, $"httprequest error occurred while DeserializeObject : {@Endpoint} response : {@content} result : {@result}");
      Console.WriteLine(e.Message + e.StackTrace);
      Console.WriteLine(content);
      return HeroResult<T>.False(content);
    }
  }

  public async Task<HeroResult<T>> GetRequestParamAsync<T>(string Endpoint, string token, Dictionary<string, string> queryStringParams)
  {
    var url = QueryHelpers.AddQueryString(Endpoint, queryStringParams);

    return await GetRequestAsync<T>(url, token);
  }

  public async Task<HeroResult<T>> PostRequestAsync<T>(string Endpoint, object content, string token)
  {
    HttpRequestMessage httpRequestMessage = new();

    httpRequestMessage.Method = new HttpMethod(HttpMethod.Post.ToString());

    httpRequestMessage.RequestUri = new Uri(Endpoint);

    var serializedObject = JsonConvert.SerializeObject(content);

    httpRequestMessage.Content = new StringContent(serializedObject);


    httpRequestMessage.Content.Headers.ContentType
        = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json);


    if (!string.IsNullOrEmpty(token))
    {
      httpRequestMessage.Headers.Authorization
     = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    var client = _httpClientFactory.CreateClient();
    var result = await client.SendAsync(httpRequestMessage);


    var responde = await result.Content.ReadAsStringAsync();

    _logger.Log(LogLevel.Information, $"calling {Endpoint} status code {result.StatusCode} response as string : {responde}");

    if (result.StatusCode == HttpStatusCode.InternalServerError ||
        result.StatusCode == HttpStatusCode.BadRequest ||
        result.StatusCode == HttpStatusCode.Unauthorized)
    {
      _logger.Log(LogLevel.Error, $"httprequest error occurred while calling post Endpoint: {Endpoint} body : {content} response : {responde} result : {result}");
      return HeroResult<T>.False(responde);
    }

    try
    {
      var res = JsonConvert.DeserializeObject<T>(responde);
      //var res = Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(responde);
      return HeroResult<T>.True(res);
    }
    catch (Exception e)
    {
      _logger.Log(LogLevel.Error, $"httprequest error occurred while DeserializeObject result: {Endpoint} body : {content} response : {responde} result : {result}");
      _logger.Log(LogLevel.Error, $"the error: {e}");
      Console.WriteLine(e.Message + e.StackTrace);
      Console.WriteLine(responde);
      return HeroResult<T>.False(responde);
    }
  }


  public async Task<HeroResult<T>> PostRequestAsyncForm<T>(string Endpoint, HttpModelDTO content, string token, bool isAttachments = false)
  {
    HttpRequestMessage httpRequestMessage = new();

    httpRequestMessage.Method = new HttpMethod(HttpMethod.Post.ToString());

    httpRequestMessage.RequestUri = new Uri(Endpoint);

    if (isAttachments)
    {
      var multipartContent = new MultipartFormDataContent();

      multipartContent.Add(new StringContent(JsonConvert.SerializeObject(content.Data)), "data");

      foreach (var item in content.NewFiles)
      {
        multipartContent.Add(new StreamContent(item.File.OpenReadStream()), item.NameOfFile, item.File.FileName);
      }

      httpRequestMessage.Content = multipartContent;
    }
    else
    {
      var serializedObject = JsonConvert.SerializeObject(content.Data);

      httpRequestMessage.Content = new StringContent(serializedObject);

      httpRequestMessage.Content.Headers.ContentType
      = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json);
    }

    if (!string.IsNullOrEmpty(token))
    {
      httpRequestMessage.Headers.Authorization
     = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    var client = _httpClientFactory.CreateClient();
    var result = await client.SendAsync(httpRequestMessage);

    var responde = await result.Content.ReadAsStringAsync();

    _logger.Log(LogLevel.Information, $"calling {Endpoint} status code {result.StatusCode} response as string : {responde}");

    if (result.StatusCode == HttpStatusCode.InternalServerError ||
        result.StatusCode == HttpStatusCode.BadRequest ||
        result.StatusCode == HttpStatusCode.Unauthorized)
    {
      _logger.Log(LogLevel.Error, "httprequest error occurred while calling post Endpoint: {@Endpoint} body : {@content} response : {@responde} result : {@result}", Endpoint, content, responde, result);
      Console.WriteLine(responde);
      return HeroResult<T>.False(responde);
    }

    try
    {
      var res = JsonConvert.DeserializeObject<T>(responde);
      //var res = Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(responde);
      return HeroResult<T>.True(res);
    }
    catch (Exception e)
    {
      _logger.Log(LogLevel.Error, $"httprequest error occurred while DeserializeObject result: {@Endpoint} body : {@content} response : {@responde} result : {@result}");
      Console.WriteLine(e.Message + e.StackTrace);
      Console.WriteLine(responde);
      return HeroResult<T>.False(responde);
    }
  }


  public Dictionary<string, IFormFile> PrepareProperties<T>(Type classType, T model)
  {
    Dictionary<string, IFormFile> propertiesAsDictionary = new Dictionary<string, IFormFile>();

    var properties = classType.GetProperties();

    foreach (PropertyInfo info in properties)
    {
      var x = info.GetValue(model, null);

      if (x is IFormFile)
      {
        IFormFile xx = x as IFormFile;

        propertiesAsDictionary.Add(info.Name, xx);
      }

    }
    return propertiesAsDictionary;

  }
}

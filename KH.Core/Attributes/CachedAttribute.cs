
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Security.Claims;
//using System.Text.Json.Serialization;

//namespace CA.Application.Attributes
//{
//  //move to infra
//  public interface IResponseCacheService
//  {
//    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
//    Task CacheResponseAsync(string cacheKey, ClaimsIdentity? response, TimeSpan timeToLive);
//    Task<string> GetCachedResponseAsync(string cacheKey);
//    Task<bool> DestoryCachedResponseAsync(string cacheKey);
//  }

//  public class ResponseCacheService : IResponseCacheService
//  {
//    private readonly IDatabase _database;

//    public ResponseCacheService(IConnectionMultiplexer redis)
//    {
//      _database = redis.GetDatabase();
//    }

//    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
//    {
//      if (response == null) return;

//      var options = new JsonSerializerOptions
//      {
//        PropertyNameCaseInsensitive = true,
//        IncludeFields = true,
//        PropertyNamingPolicy = null,
//        ReferenceHandler = ReferenceHandler.Preserve,
//        WriteIndented = true
//      };

//      var serializeResponse = JsonSerializer.Serialize(response, options);

//      await _database.StringSetAsync(cacheKey, serializeResponse, timeToLive);
//    }

//    public async Task CacheResponseAsync(string cacheKey, ClaimsIdentity? response, TimeSpan timeToLive)
//    {
//      try
//      {
//        if (response == null) return;

//        var options = new Newtonsoft.Json.JsonSerializerSettings
//        {
//          NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
//          MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
//          ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
//        };

//        var serializeResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response, options);

//        await _database.StringSetAsync(cacheKey, serializeResponse, timeToLive);
//      }
//      catch (Exception)
//      {
//      }
//    }

//    public async Task<string> GetCachedResponseAsync(string cacheKey)
//    {
//      var cachedResponse = await _database.StringGetAsync(cacheKey);

//      if (cachedResponse.IsNullOrEmpty) return null;

//      return cachedResponse;
//    }

//    public async Task<bool> DestoryCachedResponseAsync(string cacheKey)
//    {
//      try
//      {
//        var cachedResponse = await _database.KeyDeleteAsync(cacheKey);
//        return cachedResponse;
//      }
//      catch (Exception)
//      {
//      }

//      return false;

//    }
//  }
//  public class CachedAttribute : Attribute, IAsyncActionFilter
//  {
//    private readonly int _timeToLiveSeconds;

//    public CachedAttribute(int timeToLiveSeconds)
//    {
//      _timeToLiveSeconds = timeToLiveSeconds;
//    }

//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//      var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

//      var cacheKey = generateCacheKeyFromRequest(context.HttpContext.Request);

//      //var test  = await cachedService.DestoryCachedResponseAsync(cacheKey);

//      var cachedResponse = await cachedService.GetCachedResponseAsync(cacheKey);

//      if (!string.IsNullOrEmpty(cachedResponse))
//      {
//        var contentResult = new ContentResult
//        {
//          Content = cachedResponse,
//          ContentType = "application/json",
//          StatusCode = 200
//        };

//        context.Result = contentResult;

//        return;
//      }

//      var executedContext = await next();

//      if (executedContext.Result is OkObjectResult okObjectResult)
//      {
//        await cachedService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
//      }
//    }

//    string generateCacheKeyFromRequest(HttpRequest request)
//    {
//      var keyBuilder = new StringBuilder();

//      keyBuilder.Append($"{request.Path}");

//      foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
//      {
//        keyBuilder.Append($"|{key}-{value}");
//      }

//      return keyBuilder.ToString();
//    }
//  }
//}

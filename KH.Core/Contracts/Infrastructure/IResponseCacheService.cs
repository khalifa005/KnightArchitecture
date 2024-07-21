using System.Security.Claims;

namespace KH.Helper.Contracts.Infrastructure
{
  public interface IResponseCacheService
  {
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task CacheResponseAsync(string cacheKey, ClaimsIdentity? response, TimeSpan timeToLive);
    Task<string> GetCachedResponseAsync(string cacheKey);
    Task<bool> DestoryCachedResponseAsync(string cacheKey);
  }
}

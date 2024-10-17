using Microsoft.Extensions.Caching.Memory;

namespace KH.BuildingBlocks.Cache.Interfaces;

public class MemoryCacheService : ICacheService
{
  private readonly IMemoryCache _memoryCache;
  private readonly CacheSettings _cacheSettings;
  private MemoryCacheEntryOptions _cacheOptions;
  public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
  {
    _memoryCache = memoryCache;
    _cacheSettings = cacheSettings.Value;
    if (_cacheSettings != null)
    {
      _cacheOptions = new MemoryCacheEntryOptions
      {
        AbsoluteExpiration = DateTime.Now.AddHours(_cacheSettings.AbsoluteExpirationInHours),
        Priority = CacheItemPriority.High,
        SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes)
      };
    }
  }
  public bool TryGet<T>(string cacheKey, out T value)
  {
    _memoryCache.TryGetValue(cacheKey, out value);
    if (value == null) return false;
    else return true;
  }
  public T Set<T>(string cacheKey, T value)
  {
    return _memoryCache.Set(cacheKey, value, _cacheOptions);
  }
  public void Remove(string cacheKey)
  {
    _memoryCache.Remove(cacheKey);
  }
}

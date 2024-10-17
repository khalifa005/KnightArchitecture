namespace KH.BuildingBlocks.Cache.Interfaces;

public interface ICacheService
{
  bool TryGet<T>(string cacheKey, out T value);
  T Set<T>(string cacheKey, T value);
  void Remove(string cacheKey);
}

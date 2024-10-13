using System.Collections.Concurrent;

namespace KH.BuildingBlocks.Auth;

/// <summary>
/// 'GetOrAdd' call on the ConcurrentDictionary is not thread safe and we might end up creating the GetterInfo more than
/// once. To prevent this Lazy<> is used. In the worst case multiple Lazy<> objects are created for multiple
/// threads but only one of the objects succeeds in creating a GetterInfo.
///
/// The LockingConcurrentDictionary<TKey, TValue> class provided is a thread-safe dictionary
/// designed to handle concurrent operations efficiently using the ConcurrentDictionary<TKey, Lazy<TValue>>
/// from .NET. Here’s an explanation of the class and when to use it:
/// example https://chatgpt.com/c/63d382a7-5761-4833-9fb2-580c06405143
/// </summary>
public class LockingConcurrentDictionary<TKey, TValue>
{
  private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary;

  public LockingConcurrentDictionary()
  {
    _dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
  }

  public LockingConcurrentDictionary(IEqualityComparer<TKey> comparer)
  {
    _dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(comparer);
  }

  public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
  {
    var lazyResult = _dictionary.GetOrAdd(key,
      k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication));
    return lazyResult.Value;
  }

  public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
  {
    var lazyResult = _dictionary.AddOrUpdate(
      key,
      new Lazy<TValue>(() => addValue),
      (k, currentValue) => new Lazy<TValue>(() => updateValueFactory(k, currentValue.Value),
                          LazyThreadSafetyMode.ExecutionAndPublication));
    return lazyResult.Value;
  }

  public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
  {
    var lazyResult = _dictionary.AddOrUpdate(
      key,
      k => new Lazy<TValue>(() => addValueFactory(k)),
      (k, currentValue) => new Lazy<TValue>(() => updateValueFactory(k, currentValue.Value),
                          LazyThreadSafetyMode.ExecutionAndPublication));
    return lazyResult.Value;
  }

  public int Count => _dictionary.Count;
}

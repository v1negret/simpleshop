using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SimpleShop.Cache.Interfaces;

namespace SimpleShop.Cache;

public class DistributedCacheService : IDistributedCacheService
{
    private readonly IDistributedCache _cache;

    public DistributedCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task<T?> GetData<T>(string key)
    {
        var exist = await _cache.GetStringAsync(key);
        if (String.IsNullOrEmpty(exist)) return default;
        
        return JsonSerializer.Deserialize<T>(exist);
    }

    public async Task SetData<T>(string key, T value, TimeSpan expTime)
    {
        var str = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key,str, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expTime
        });

    }

    public async Task RemoveData(string key)
    {
        var str = await _cache.GetStringAsync(key);
        if (!String.IsNullOrEmpty(str))
        {
            await _cache.RefreshAsync(str);
        }
    }
}
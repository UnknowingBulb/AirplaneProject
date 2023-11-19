using AirplaneProject.Database.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AirplaneProject.Infrastructure.Database.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public Task SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = expirationTime
            };
            return _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), cacheOptions);
        }
    }
}
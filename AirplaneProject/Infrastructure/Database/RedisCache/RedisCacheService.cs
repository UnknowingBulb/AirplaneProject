using AirplaneProject.Database.Cache;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AirplaneProject.Infrastructure.Database.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private IDatabase _db;
        public RedisCacheService()
        {
            ConfigureRedis();
        }
        private void ConfigureRedis()
        {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
    }
}
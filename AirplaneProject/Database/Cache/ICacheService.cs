namespace AirplaneProject.Database.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Получить данные
        /// </summary>
        Task<T> GetDataAsync<T>(string key);

        /// <summary>
        /// Сохранить данные в кэш
        /// </summary>
        Task SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
    }
}
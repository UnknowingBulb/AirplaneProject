using AirplaneProject.Domain.Entities;

namespace AirplaneProject.Application.Interfaces.DbData
{
    /// <summary>
    /// Работа с БД по пользователям (клиентам/сотрудникам)
    /// </summary>
    public interface IPassengerDb
    {
        /// <summary>
        /// Получить пассажира
        /// </summary>
        public ValueTask<Passenger?> GetAsync(Guid id);

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        public Task<List<Passenger>> GetUserPassengersAsync(Guid userId);
    }
}

using AirplaneProject.Domain.Entities;

namespace AirplaneProject.Application.Interfaces.DbData
{
    /// <summary>
    /// Работа с БД по рейсам
    /// </summary>
    public interface IFlightDb
    {
        /// <summary>
        /// Получить рейс с заполненным Orders
        /// </summary>
        public Task<Flight?> GetAsync(Guid id);

        /// <summary>
        /// Получить список неотправившихся рейсов
        /// </summary>
        public Task<List<Flight>> GetUpcomingFlightsAsync();
    }
}

using AirplaneProject.Domain.Entities;

namespace AirplaneProject.Application.Interfaces.DbData
{
    /// <summary>
    /// Заполнить БД какими-нибудь данными
    /// </summary>
    public interface ISpawnDataDb
    {
        Task<bool> IsNotEmptyAsync();

        Task<List<User>> AddUsersAsync();

        public Task<Passenger> AddPassengerAsync(User user);

        Task<List<Flight>> AddFlightsAsync();

        Task AddOrdersAsync(Guid userId, Guid flightId, Guid passengerId);
    }
}

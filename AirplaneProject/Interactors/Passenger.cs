using AirplaneProject.Objects;
using AirplaneProject.Database;
using AirplaneProject.Database.DbData;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneProject.Interactors
{
    [Authorize]
    public class Passenger
    {
        private readonly PassengerDb _passengerDb;
        public Passenger(ApplicationDbContext dbContext)
        {
            _passengerDb = new PassengerDb(dbContext);
        }

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        public Task<List<Objects.Passenger>> GetUserPassengersAsync(Guid userId)
        {
            return _passengerDb.GetUserPassengersAsync(userId);
        }

        /// <summary>
        /// Создать пассажира (отлеживается БД, но не сохраняется)
        /// </summary>
        public Task CreateAsync(Objects.Passenger passenger)
        {
            return _passengerDb.CreateAsync(passenger);
        }
    }
}

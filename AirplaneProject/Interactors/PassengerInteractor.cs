using AirplaneProject.Objects;
using AirplaneProject.Database;
using AirplaneProject.Database.DbData;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneProject.Interactors
{
    [Authorize]
    public class PassengerInteractor
    {
        private readonly PassengerDb _passengerDb;
        public PassengerInteractor(ApplicationDbContext dbContext)
        {
            _passengerDb = new PassengerDb(dbContext);
        }

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        public Task<List<PassengerModel>> GetUserPassengersAsync(Guid userId)
        {
            return _passengerDb.GetUserPassengersAsync(userId);
        }

        /// <summary>
        /// Сохранить пассажира
        /// </summary>
        public Task CreateAsync(PassengerModel passenger)
        {
            return _passengerDb.SaveAsync(passenger);
        }
    }
}

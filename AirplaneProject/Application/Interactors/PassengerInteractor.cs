using AirplaneProject.Database.DbData;
using AirplaneProject.Domain.Entities;
using AirplaneProject.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneProject.Application.Interactors
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
        public Task<List<Passenger>> GetUserPassengersAsync(Guid userId)
        {
            return _passengerDb.GetUserPassengersAsync(userId);
        }
    }
}

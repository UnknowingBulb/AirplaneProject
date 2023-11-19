using AirplaneProject.Application.Interfaces.DbData;
using AirplaneProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneProject.Application.Interactors
{
    [Authorize]
    public class PassengerInteractor
    {
        private readonly IPassengerDb _passengerDb;
        public PassengerInteractor(IPassengerDb passengerDb)
        {
            _passengerDb = passengerDb;
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

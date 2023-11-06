using AirplaneProject.Database;
using AirplaneProject.Database.Cache;
using AirplaneProject.Database.DbData;
using AirplaneProject.Objects;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AirplaneProject.Interactors
{
    public class FlightInteractor
    {
        private readonly FlightDb _flightDb;
        private readonly ICacheService _cacheService;
        public FlightInteractor(ApplicationDbContext dbContext, ICacheService cacheService)
        {
            _flightDb = new FlightDb(dbContext);
            _cacheService = cacheService;
        }

        /// <summary>
        /// Получить рейс с заполненным Orders
        /// </summary>
        public async Task<Result<Flight>> GetAsync(Guid flightId)
        {
            var flight = await _flightDb.GetAsync(flightId);
            if (flight == null)
            {
                return Result.Fail("Не удалось найти рейс");
            }
            return Result.Ok(flight);
        }

        /// <summary>
        /// Получить список неотправившихся рейсов
        /// </summary>
        public async Task<List<Flight>> GetUpcomingFlightsAsync()
        {
            var cacheData = await _cacheService.GetDataAsync<List<Flight>>("flight");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = await _flightDb.GetUpcomingFlightsAsync();
            await _cacheService.SetDataAsync("flight", cacheData, expirationTime);

            return cacheData;
        }

        /// <summary>
        /// Получить список незанятых мест на рейсе
        /// </summary>
        public IEnumerable<int> GetEmptySeatNumbers(Flight flight)
        {
            var totalSeatCount = flight.SeatingCapacity;

            var seatNumbers = Enumerable.Range(1, totalSeatCount).ToList();
            foreach (var order in flight.Orders)
            {
                if (order.IsActive)
                    foreach (var seat in order.SeatReserves)
                        seatNumbers.Remove(seat.SeatNumber);
            }

            return seatNumbers;
        }

        /// <summary>
        /// Проверка, что указанные места на рейс еще не заняты
        /// </summary>
        public Result IsSeatsEmpty(Flight flight, IEnumerable<int> seatNumbers)
        {
            var emptySeatsResult = GetEmptySeatNumbers(flight);
            
            var notEmptySeats = seatNumbers.Except(emptySeatsResult);

            if (notEmptySeats.Count() == 0)
                return Result.Ok();

            var errorMessage = new StringBuilder();

            errorMessage.Append("Эти места уже зарезервированы ранее: ");
            foreach (var seat in notEmptySeats)
            {
                errorMessage.Append($"{notEmptySeats} ");
            }
            errorMessage.AppendLine("/r/n Замените их на другие и попробуйте снова");
            return Result.Fail(errorMessage.ToString());
        }

    }
}

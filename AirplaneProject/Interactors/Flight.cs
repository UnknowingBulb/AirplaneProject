using AirplaneProject.Objects;
using AirplaneProject.Database;
using AirplaneProject.Database.DbData;
using FluentResults;
using System.Text;

namespace AirplaneProject.Interactors
{
    public class Flight
    {
        private readonly FlightDb _flightDb;
        public Flight(ApplicationDbContext dbContext)
        {
            _flightDb = new FlightDb(dbContext);
        }

        /// <summary>
        /// Получить рейс с заполненным Orders
        /// </summary>
        public async Task<Result<Objects.Flight>> GetAsync(Guid flightId)
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
        public Task<List<Objects.Flight>> GetUpcomingFlightsAsync()
        {
            return _flightDb.GetUpcomingFlightsAsync();
        }

        /// <summary>
        /// Получить список незанятых мест на рейсе
        /// </summary>
        public IEnumerable<int> GetEmptySeatNumbers(Objects.Flight flight)
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
        public Result IsSeatsEmpty(Objects.Flight flight, IEnumerable<int> seatNumbers)
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

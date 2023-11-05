using AiplaneProject.Objects;
using AirplaneProject.Database;
using AirplaneProject.Database.DbData;
using FluentResults;

namespace AirplaneProject.Interactors
{
    public class FlightInteractor
    {
        private readonly FlightDb _flightDb;
        public FlightInteractor(ApplicationDbContext dbContext)
        {
            _flightDb = new FlightDb(dbContext);
        }

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
        public Task<List<Flight>> GetUpcomingFlightsAsync()
        {
            return _flightDb.GetUpcomingFlightsAsync();
        }


        /// <summary>
        /// Получить список незанятых мест на рейсе
        /// </summary>
        public async Task<Result<List<int>>> GetEmptySeatNumbers(Guid flightId)
        {
            var flightResult = await GetAsync(flightId);
            if (flightResult.IsFailed)
                return Result.Fail("Не удалось найти рейс");

            var totalSeatCount = flightResult.Value.SeatingCapacity;
            var seatNumbers = Enumerable.Range(1, totalSeatCount + 1).ToList();
            foreach (var order in flightResult.Value.Orders)
            {
                if (order.IsActive)
                    foreach (var seat in order.SeatReserves)
                        seatNumbers.Remove(seat.SeatNumber);
            }

            return Result.Ok(seatNumbers);
        }

        /// <summary>
        /// Проверка, что указанное место на рейс еще не занято
        /// </summary>
        public async Task<Result<bool>> IsSeatEmptyAsync(Guid flightId, int seatNumber)
        {
            var emptySeatsResult = await GetEmptySeatNumbers(flightId);
            if (emptySeatsResult.IsFailed)
                return Result.Fail(emptySeatsResult.Errors[0]);
            
            var isSeatEmpty = emptySeatsResult.Value.Contains(seatNumber);

            return Result.Ok(isSeatEmpty);
        }

    }
}

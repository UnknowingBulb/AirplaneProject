using AiplaneProject.Objects;
using AirplaneProject.Database.DatabaseContextes;

namespace AirplaneProject.Interacotrs
{
    public class FlightInteractor
    {
        private readonly FlightDbContext _flightDbContext;
        public FlightInteractor(FlightDbContext flightDbContext)
        {
            _flightDbContext = flightDbContext;
        }

        /// <summary>
        /// Получить список неотправившихся рейсов
        /// </summary>
        public IEnumerable<Flight> GetUpcomingFlights()
        {
            return _flightDbContext.Flight.Where(f => f.DepartureDateTime>=DateTime.UtcNow);
        }
    }
}

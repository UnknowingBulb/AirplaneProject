using AiplaneProject.Objects;
using AirplaneProject.Database.DatabaseContextes;

namespace AirplaneProject.Interactors
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
        public IQueryable<Flight> GetUpcomingFlights()
        {
            return _flightDbContext.Flight.Where(f => f.DepartureDateTime>=DateTime.UtcNow).OrderBy(f => f.DepartureDateTime);
        }
    }
}

using AiplaneProject.Objects;
using AirplaneProject.Database;
using AirplaneProject.Database.DbData;

namespace AirplaneProject.Interactors
{
    public class FlightInteractor
    {
        private readonly FlightDb _flightDb;
        public FlightInteractor(ApplicationDbContext dbContext)
        {
            _flightDb = new FlightDb(dbContext);
        }

        /// <summary>
        /// Получить список неотправившихся рейсов
        /// </summary>
        public IQueryable<Flight> GetUpcomingFlights()
        {
            return _flightDb.GetUpcomingFlights();
        }
    }
}

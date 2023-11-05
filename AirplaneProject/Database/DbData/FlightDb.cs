using AiplaneProject.Objects;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Работа с БД по рейсам
    /// </summary>
    public class FlightDb
    {
        private readonly ApplicationDbContext _dbContext;
        public FlightDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить список неотправившихся рейсов
        /// </summary>
        public IQueryable<Flight> GetUpcomingFlights()
        {
            return _dbContext.Flight.Where(f => f.DepartureDateTime >= DateTime.UtcNow).OrderBy(f => f.DepartureDateTime);
        }
    }
}

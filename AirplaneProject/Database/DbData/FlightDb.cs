using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

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
        /// Получить рейс
        /// </summary>
        public ValueTask<Flight?> GetAsync(Guid id)
        {
            return _dbContext.Flight.FindAsync(id);
        }

        /// <summary>
        /// Получить список неотправившихся рейсов
        /// </summary>
        public async Task<List<Flight>> GetUpcomingFlightsAsync()
        {
            return await _dbContext.Flight.Where(f => f.DepartureDateTime >= DateTime.UtcNow)
                .OrderBy(f => f.DepartureDateTime)
                .ToListAsync();
        }
    }
}

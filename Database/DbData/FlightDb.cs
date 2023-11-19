using AirplaneProject.Application.Interfaces;
using AirplaneProject.Application.Interfaces.DbData;
using AirplaneProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Работа с БД по рейсам
    /// </summary>
    public class FlightDb : IFlightDb
    {
        private readonly IApplicationDbContext _dbContext;
        public FlightDb(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить рейс с заполненным Orders
        /// </summary>
        public Task<Flight?> GetAsync(Guid id)
        {
            return _dbContext.Flight
                .Include(f => f.Orders)
                .ThenInclude(o => o.SeatReserves)
                .FirstOrDefaultAsync(f => f.Id == id);
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

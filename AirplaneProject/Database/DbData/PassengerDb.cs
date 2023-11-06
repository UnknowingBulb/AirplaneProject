using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Работа с БД по пользователям (клиентам/сотрудникам)
    /// </summary>
    public class PassengerDb
    {
        private readonly ApplicationDbContext _dbContext;
        public PassengerDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить пассажира
        /// </summary>
        public ValueTask<Passenger?> GetAsync(Guid id)
        {
            return _dbContext.Passenger.FindAsync(id);
        }

        /// <summary>
        /// Пассажиры, созданные пользователем
        /// </summary>
        public Task<List<Passenger>> GetUserPassengersAsync(Guid userId)
        {
            return _dbContext.Passenger.Where(passenger => passenger.UserId == userId).ToListAsync();
        }
    }
}

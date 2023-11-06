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
        {/*
            var passengerList = await _dbContext.Passenger.Where(passenger =>  passenger.UserId == userId).ToListAsync();
            foreach (var passenger in passengerList)
            {
                _dbContext.Entry(passenger).State = EntityState.Detached;
            }
            return passengerList;*/
            return _dbContext.Passenger.Where(passenger => passenger.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Создать пассажира (отслеживать в БД)
        /// </summary>
        public async Task CreateAsync(Passenger passenger)
        {
            await _dbContext.Passenger.AddAsync(passenger);
        }
    }
}

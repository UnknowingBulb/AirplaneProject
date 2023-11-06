using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Работа с БД по заказам
    /// </summary>
    public class OrderDb
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Сохранить заказ
        /// </summary>
        public async Task CreateAndSaveAsync(Order order)
        {
            await _dbContext.Order.AddAsync(order);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public ValueTask<Order?> GetAsync(Guid id)
        {
            return _dbContext.Order.FindAsync(id);
        }

        /// <summary>
        /// Получить список заказов определенного пользователя
        /// </summary>
        public Task<List<Order>> GetOrdersByUserAsync(Guid userId)
        {
            return _dbContext.Order.Where(order => order.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Получить список заказов пользователя по полному/частичному совпадению в ФИО
        /// </summary>
        public Task<List<Order>> GetOrdersByUserNameAsync(string name)
        {
            return _dbContext.Order.Where(order => order.User.Name.Contains(name)).ToListAsync();
        }

        /// <summary>
        /// Получить список заказов пользователя по номеру телефона
        /// </summary>
        public Task<List<Order>> GetOrdersByUserPhoneAsync(string phone)
        {
            return _dbContext.Order.Where(order => order.User.PhoneNumber == phone).ToListAsync();
        }

        /// <summary>
        /// Создать резев места (отслеживать в БД)
        /// </summary>
        public async Task CreateSeatReserveAsync(SeatReserve seatReserve)
        {
            await _dbContext.SeatReserve.AddAsync(seatReserve);
        }

        /// <summary>
        /// Создать резев места (отслеживать в БД)
        /// </summary>
        public async Task CreateSeatReserveAsync(List<SeatReserve> seatReserves)
        {
            await _dbContext.SeatReserve.AddRangeAsync(seatReserves);
        }
    }
}

using AiplaneProject.Objects;
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
        public Task SaveAsync(Order order)
        {
            _dbContext.Order.Add(order);
            return _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public ValueTask<Order?> GetOrderAsync(Guid id)
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
        /// Получить список заказов пользователя по номеру телефона
        /// </summary>
        public Task<List<Order>> GetOrdersByUserPhoneAsync(string phone)
        {
            return _dbContext.Order.Where(order => order.User.PhoneNumber == phone).ToListAsync();
        }
    }
}

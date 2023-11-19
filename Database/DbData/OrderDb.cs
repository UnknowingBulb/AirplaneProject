using AirplaneProject.Application.Interfaces;
using AirplaneProject.Application.Interfaces.DbData;
using AirplaneProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Работа с БД по заказам
    /// </summary>
    public class OrderDb : IOrderDb
    {
        private readonly IApplicationDbContext _dbContext;
        public OrderDb(IApplicationDbContext dbContext)
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
        /// Сохранить заказ
        /// </summary>
        public async Task SaveAsync(Order order)
        {
            _dbContext.Order.Update(order);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public Task<Order?> GetAsync(Guid id)
        {
            return _dbContext.Order.Include(o => o.Flight).FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Получить список заказов определенного пользователя
        /// </summary>
        public Task<List<Order>> GetOrdersByUserAsync(Guid userId)
        {
            return _dbContext.Order.Include(o => o.Flight).Where(order => order.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Получить список заказов пользователя по полному/частичному совпадению в ФИО
        /// </summary>
        public Task<List<Order>> GetOrdersByUserNameAsync(string name)
        {
            return _dbContext.Order
                .Include(o => o.Flight)
                .Include(o => o.User)
                .Where(o => o.User.Name.Contains(name))
                .ToListAsync();
        }

        /// <summary>
        /// Получить список заказов пользователя по номеру телефона
        /// </summary>
        public Task<List<Order>> GetOrdersByUserPhoneAsync(string phone)
        {
            return _dbContext.Order
                .Include(o => o.Flight)
                .Include(o => o.User)
                .Where(order => order.User.PhoneNumber.Contains(phone))
                .ToListAsync();
        }

        /// <summary>
        /// Получить список всех заказов
        /// </summary>
        public Task<List<Order>> GetOrdersAsync()
        {
            return _dbContext.Order.Include(o => o.Flight).Include(o => o.User).ToListAsync();
        }
    }
}

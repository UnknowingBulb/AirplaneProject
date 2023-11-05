using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    public class OrderDb
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Save(Order order)
        {
            _dbContext.Order.Add(order);
            return _dbContext.SaveChangesAsync();
        }

        public ValueTask<Order?> GetOrder(Guid id)
        {
            return _dbContext.Order.FindAsync(id);
        }

        public IQueryable<Order> GetOrdersByUser(Guid userId)
        {
            return _dbContext.Order.Where(order => order.UserId == userId);
        }

        public IQueryable<Order> GetOrdersByUserPhone(string phone)
        {
            return _dbContext.Order.Where(order => order.User.PhoneNumber == phone);
        }
    }
}

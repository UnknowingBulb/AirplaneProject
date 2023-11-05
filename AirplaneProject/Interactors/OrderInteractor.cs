using AiplaneProject.Objects;
using AirplaneProject.Database;
using AirplaneProject.Database.DbData;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AirplaneProject.Interactors
{
    [Authorize]
    public class OrderInteractor
    {
        private readonly OrderDb _orderDb;
        public OrderInteractor(ApplicationDbContext dbContext)
        {
            _orderDb = new OrderDb(dbContext);
        }

        public Task Create(Order order)
        {
            ValidateOrder(order);
            return _orderDb.Save(order);
        }

        public Task SetNotActive(Order order)
        {
            ValidateOrder(order);
            //TODO: check smth
            order.IsActive = false;
            return _orderDb.Save(order);
        }

        public async Task<Result<Order>> GetOrder(Guid id) 
        {
            var order = await _orderDb.GetOrder(id);
            if (order == null)
            {
                return Result.Fail("Не удалось найти заказ");
            }
            return order;
        }

        public Result<IQueryable<Order>> GetOrdersByPhone(string phoneNumber)
        {
            var orders = _orderDb.GetOrdersByUserPhone(phoneNumber);
            if (orders.IsNullOrEmpty())
            {
                return Result.Fail("Не удалось найти заказы");
            }
            return Result.Ok(orders);
        }

        public Result<IQueryable<Order>> GetOrdersByUser(Guid userId)
        {
            var orders = _orderDb.GetOrdersByUser(userId);
            if (orders.IsNullOrEmpty())
            {
                return Result.Fail("Не удалось найти заказы");
            }
            return Result.Ok(orders);
        }

        private Result ValidateOrder(Order order)
        {
            return Result.Ok();
        }
    }
}

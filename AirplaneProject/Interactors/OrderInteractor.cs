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

        /// <summary>
        /// Создать заказ
        /// </summary>
        public Task Create(Order order)
        {
            ValidateOrder(order);
            return _orderDb.SaveAsync(order);
        }

        /// <summary>
        /// Установить, что заказ неактивен
        /// </summary>
        public Task SetNotActive(Order order)
        {
            ValidateOrder(order);
            //TODO: check smth
            order.IsActive = false;
            return _orderDb.SaveAsync(order);
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public async Task<Result<Order>> GetOrder(Guid id) 
        {
            var order = await _orderDb.GetOrderAsync(id);
            if (order == null)
            {
                return Result.Fail("Не удалось найти заказ");
            }
            return order;
        }

        /// <summary>
        /// Получить заказы, которые сделал пользователь
        /// </summary>
        public Result<IQueryable<Order>> GetOrdersByUser(Guid userId)
        {
            var orders = _orderDb.GetOrdersByUser(userId);
            if (orders.IsNullOrEmpty())
            {
                return Result.Fail("Не удалось найти заказы");
            }
            return Result.Ok(orders);
        }

        /// <summary>
        /// Получить заказы по номеру телефона пользователя
        /// </summary>
        public Result<IQueryable<Order>> GetOrdersByPhone(string phoneNumber)
        {
            var orders = _orderDb.GetOrdersByUserPhone(phoneNumber);
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

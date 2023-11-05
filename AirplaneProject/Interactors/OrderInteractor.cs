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
        public async Task<Result> CreateAsync(Order order)
        {
            var isOrderValid = ValidateOrder(order);
            if (isOrderValid.IsFailed)
                return isOrderValid;
            await _orderDb.SaveAsync(order);
            return Result.Ok();
        }

        /// <summary>
        /// Установить, что заказ неактивен
        /// </summary>
        public Task SetNotActiveAsync(Order order)
        {
            ValidateOrder(order);
            //TODO: check smth
            order.IsActive = false;
            return _orderDb.SaveAsync(order);
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public async Task<Result<Order>> GetOrderAsync(Guid id) 
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
        public async Task<Result<List<Order>>> GetOrdersByUserAsync(Guid userId)
        {
            var orders = await _orderDb.GetOrdersByUserAsync(userId);
            if (orders.IsNullOrEmpty())
            {
                return Result.Fail("Не удалось найти заказы");
            }
            return Result.Ok(orders);
        }

        /// <summary>
        /// Получить заказы по номеру телефона пользователя
        /// </summary>
        public async Task<Result<List<Order>>> GetOrdersByPhoneAsync(string phoneNumber)
        {
            var orders = await _orderDb.GetOrdersByUserPhoneAsync(phoneNumber);
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

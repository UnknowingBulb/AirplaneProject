using AirplaneProject.Objects;
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
        private readonly FlightInteractor _flightInteractor;
        public OrderInteractor(ApplicationDbContext dbContext)
        {
            _orderDb = new OrderDb(dbContext);
            _flightInteractor = new FlightInteractor(dbContext);
        }

        /// <summary>
        /// Создать заказ
        /// </summary>
        public async Task<Result> CreateAsync(Order order)
        {
            var isOrderValid = ValidateOrderAsync(order);
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
            ValidateOrderToChange(order);
            order.IsActive = false;
            return _orderDb.SaveAsync(order);
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public async Task<Result<Order>> GetAsync(Guid id)
        {
            var order = await _orderDb.GetAsync(id);
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
        /// Получить заказы, которые сделал пользователь по полному/частичному совпадению ФИО
        /// </summary>
        public async Task<Result<List<Order>>> GetOrdersByUserNameAsync(string userName)
        {
            var orders = await _orderDb.GetOrdersByUserNameAsync(userName);
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

        private Result ValidateOrderAsync(Order order)
        {
            var result = ValidateOrderToChange(order);
            if (result.IsFailed)
                return result;

            if (order.SeatReserves.IsNullOrEmpty())
                result = Result.Merge(result, Result.Fail("Пуста информация по местам. Заполните и попробуйте снова"));

            var orderSeatNumbers = order.SeatReserves.Select(sr => sr.SeatNumber);
            var isSeatsEmptyResult = _flightInteractor.IsSeatsEmpty(order.Flight, orderSeatNumbers);
            if (isSeatsEmptyResult.IsFailed)
                result = Result.Merge(result, isSeatsEmptyResult);

            return result;
        }

        private Result ValidateOrderToChange(Order order)
        {
            if (order == null)
                return Result.Fail("Данные пусты. Произошла ошибка. Попробуйте позже");

            if (order.Flight.DepartureDateTime < DateTime.UtcNow)
                return Result.Fail("Время отправки рейса уже прошло. Выберите другой рейс");
            
            return Result.Ok();
        }
    }
}

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

            await _orderDb.CreateAndSaveAsync(order);
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
        /// Получить все заказы
        /// </summary>
        public Task<List<Order>> GetOrdersAsync()
        {
            return _orderDb.GetOrdersAsync();
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

        /// <summary>
        /// Создать резев места (отслеживать в БД)
        /// </summary>
        public Task CreateSeatReserveAsync(SeatReserve seatReserve)
        {
            return _orderDb.CreateSeatReserveAsync(seatReserve);
        }

        /// <summary>
        /// Создать резев места (отслеживать в БД)
        /// </summary>
        public Task CreateSeatReserveAsync(List<SeatReserve> seatReserves)
        {
            return _orderDb.CreateSeatReserveAsync(seatReserves);
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

            var hasDuplicatePassengers = order.SeatReserves.DistinctBy(sr => sr.PassengerId).Count() != order.SeatReserves.Count;
            if (hasDuplicatePassengers)
                result = Result.Merge(result, Result.Fail("Есть повторяющиеся пассажиры. Нельзя брать несколько мест на одного пассажира"));

            var hasDuplicateSeatNumber = order.SeatReserves.DistinctBy(sr => sr.SeatNumber).Count() != order.SeatReserves.Count;
            if (hasDuplicateSeatNumber)
                result = Result.Merge(result, Result.Fail("В заказе есть повторяющиеся места. Они не должны повторяться"));

            var hasEmptyDataPassengers = order.SeatReserves.Any(sr => sr.Passenger != null && (sr.Passenger.Name == null
            || sr.Passenger.PassportData == null));
            if (hasEmptyDataPassengers)
                result = Result.Merge(result, Result.Fail("Не заполнены данные одного или нескольких пользователей. Все поля обязательны для заполнения"));

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

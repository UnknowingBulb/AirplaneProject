using AirplaneProject.Interactors;
using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Xml;

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
        public async Task CreateAndSaveAsync(Objects.Order order)
        {
            //TODO: проверить и мб удалить
            // Этот черт может пытаться добавить пассажира, даже если он уже существует
            // Поэтому действуем на опережение и говорим ему, что такие пассажиры могут быть сохранены
            foreach (var seatReserve in order.SeatReserves)
            {
                var passengerList2 = _dbContext.Passenger.Where(passenger => passenger.Id == seatReserve.PassengerId).ToList();
                foreach (var passenger in passengerList2)
                {
                    _dbContext.Entry(passenger).State = EntityState.Detached;
                }
                
            }

            await _dbContext.Order.AddAsync(order);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получить заказ
        /// </summary>
        public ValueTask<Objects.Order?> GetAsync(Guid id)
        {
            return _dbContext.Order.FindAsync(id);
        }

        /// <summary>
        /// Получить список заказов определенного пользователя
        /// </summary>
        public Task<List<Objects.Order>> GetOrdersByUserAsync(Guid userId)
        {
            return _dbContext.Order.Where(order => order.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Получить список заказов пользователя по полному/частичному совпадению в ФИО
        /// </summary>
        public Task<List<Objects.Order>> GetOrdersByUserNameAsync(string name)
        {
            return _dbContext.Order.Where(order => order.User.Name.Contains(name)).ToListAsync();
        }

        /// <summary>
        /// Получить список заказов пользователя по номеру телефона
        /// </summary>
        public Task<List<Objects.Order>> GetOrdersByUserPhoneAsync(string phone)
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
            /*
            //TODO: проверить и мб удалить
            // Этот черт может пытаться добавить пассажира, даже если он уже существует
            // Поэтому действуем на опережение и говорим ему, что такие пассажиры могут быть сохранены
            foreach (var seatReserve in seatReserves)
            {
                var passengerList2 = _dbContext.Passenger.Where(passenger => passenger.Id == seatReserve.PassengerId).ToList();
                foreach (var passenger in passengerList2)
                {
                    _dbContext.Entry(passenger).State = EntityState.Detached;
                }

            }*/
            _dbContext.SeatReserve.UpdateRange(seatReserves);
            await _dbContext.SeatReserve.AddRangeAsync(seatReserves);
        }
    }
}

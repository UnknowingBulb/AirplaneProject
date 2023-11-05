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
        public async Task SaveAsync(Order order)
        {
            // Этот черт может пытаться добавить пассажира, даже если он уже существует
            // Поэтому действуем на опережение и говорим ему, что такие пассажиры могут быть сохранены
            foreach (var seatReserve in order.SeatReserves)
            {
                if (_dbContext.Passenger.Any(p => p.Id == seatReserve.Passenger.Id))
                {
                    _dbContext.Attach(seatReserve.Passenger);
                   // _dbContext.Passenger.Ob.ChangeObjectState(myEntity, EntityState.Modified);
                }
                else
                {
                    if(seatReserve.Passenger.Name.IsNullOrEmpty())
                    {
                        continue;
                    }
                    _dbContext.Add(seatReserve.Passenger);
                }
            }
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
    }
}

using AirplaneProject.Domain.Entities;

namespace AirplaneProject.Application.Interfaces.DbData
{
    /// <summary>
    /// Работа с БД по заказам
    /// </summary>
    public interface IOrderDb
    {
        /// <summary>
        /// Сохранить заказ
        /// </summary>
        public Task CreateAndSaveAsync(Order order);

        /// <summary>
        /// Сохранить заказ
        /// </summary>
        public Task SaveAsync(Order order);

        /// <summary>
        /// Получить заказ
        /// </summary>
        public Task<Order?> GetAsync(Guid id);

        /// <summary>
        /// Получить список заказов определенного пользователя
        /// </summary>
        public Task<List<Order>> GetOrdersByUserAsync(Guid userId);

        /// <summary>
        /// Получить список заказов пользователя по полному/частичному совпадению в ФИО
        /// </summary>
        public Task<List<Order>> GetOrdersByUserNameAsync(string name);

        /// <summary>
        /// Получить список заказов пользователя по номеру телефона
        /// </summary>
        public Task<List<Order>> GetOrdersByUserPhoneAsync(string phone);

        /// <summary>
        /// Получить список всех заказов
        /// </summary>
        public Task<List<Order>> GetOrdersAsync();
    }
}

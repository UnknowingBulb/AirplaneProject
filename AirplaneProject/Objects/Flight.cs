namespace AiplaneProject.Objects
{
    /// <summary>
    /// Рейс
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Идентификатор рейса
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Дата-время отправления
        /// </summary>
        public DateTime DepartureDateTime { get; set; }
        /// <summary>
        /// Общее количество мест на рейс
        /// </summary>
        public int SeatingCapacity { get; set; }
        /// <summary>
        /// Заказы на данный рейс
        /// </summary>
        public List<Order> Orders { get; set; } = new();
    }
}

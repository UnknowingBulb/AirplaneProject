namespace AirplaneProject.Objects
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
        /// Цена билета
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Место отправления
        /// </summary>
        public string DepartureLocation { get; set; }
        /// <summary>
        /// Место прибытия
        /// </summary>
        public string DestinationLocation { get; set; }
        /// <summary>
        /// Заказы на данный рейс
        /// </summary>
        public List<Order> Orders { get; set; } = new();
    }
}

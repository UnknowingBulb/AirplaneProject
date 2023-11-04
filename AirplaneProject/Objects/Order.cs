namespace AiplaneProject.Objects
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор рейса
        /// </summary>
        public Guid FlightId { get; set; }
        /// <summary>
        /// Идентификатор клиента, оформившего заказ
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Стоимость заказа
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Пользователь-автор заказа
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Информация о резерве мест в заказе
        /// </summary>
        public List<SeatReserve> SeatReserves { get; set; } = new();
    }
}

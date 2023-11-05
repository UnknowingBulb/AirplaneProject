namespace AirplaneProject.Objects
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class OrderModel
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
        /// Активен ли заказ (не отменен)
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Пользователь-автор заказа
        /// </summary>
        public virtual UserModel User { get; set; }
        /// <summary>
        /// Информация о резерве мест в заказе
        /// </summary>
        public virtual List<SeatReserveModel> SeatReserves { get; set; } = new();

        /// <summary>
        /// Информация по рейсу, на который оформлен заказ
        /// </summary>
        public virtual FlightModel Flight { get; set; }
    }
}

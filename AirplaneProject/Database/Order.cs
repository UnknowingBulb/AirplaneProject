namespace AiplaneProject.Database
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public Guid Guid;
        /// <summary>
        /// Идентификатор рейса
        /// </summary>
        public Guid FligthId;
        /// <summary>
        /// Идентификатор клиента, оформившего заказ
        /// </summary>
        public Guid ClientId;
        /// <summary>
        /// Информация о резерве мест в заказе
        /// </summary>
        public List<SeatReserve> SeatReserveInfo;
    }
}

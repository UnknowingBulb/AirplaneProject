namespace AirplaneProject.Domain.Entities
{
    /// <summary>
    /// Информация по резерву места на рейс
    /// </summary>
    public class SeatReserve
    {
        /// <summary>
        /// Идентификатор резерва
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор заказа, в котором действует резерв
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Идентификатор пассажира
        /// </summary>
        public Guid PassengerId { get; set; }
        /// <summary>
        /// Инфо о пассажире
        /// </summary>
        public virtual Passenger Passenger { get; set; }
        /// <summary>
        /// Номер занятого места
        /// </summary>
        public int SeatNumber { get; set; }
    }
}

namespace AiplaneProject.Models
{
    /// <summary>
    /// Информация по резерву места на рейс
    /// </summary>
    public class SeatReserve
    {
        /// <summary>
        /// Идентификатор резерва
        /// </summary>
        public Guid Guid;
        /// <summary>
        /// Идентификатор заказа, в котором действует резерв
        /// </summary>
        public Guid OrderGuid;
        /// <summary>
        /// Идентификатор пассажира
        /// </summary>
        public Guid PassengerGuid;
        /// <summary>
        /// Номер занятого места
        /// </summary>
        public int SeatNumber;
    }
}

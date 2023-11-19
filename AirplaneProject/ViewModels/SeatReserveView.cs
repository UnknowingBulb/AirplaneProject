using AirplaneProject.Domain.Entities;

namespace AirplaneProject.ViewModels
{
    /// <summary>
    /// Модель данных по резерву места на рейс для страницы
    /// </summary>
    public class SeatReserveView
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
        public PassengerView Passenger { get; set; }
        /// <summary>
        /// Номер занятого места
        /// </summary>
        public int SeatNumber { get; set; }

        public SeatReserve ToSeatReserve(Guid userId)
        {
            var passenger = Passenger.ToPassenger(userId);
            return new SeatReserve
            {
                Id = Id,
                OrderId = OrderId,
                // грязный хак, чтобы он не добавлял записи, которые уже есть в БД
                Passenger = passenger.Value == false ? passenger.Key : null,
                PassengerId = passenger.Key.Id,
                SeatNumber = SeatNumber
            };
        }
    }
}

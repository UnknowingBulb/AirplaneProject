using AirplaneProject.Domain.Entities;

namespace AirplaneProject.ViewModels
{
    /// <summary>
    /// Модель пассажира для страницы
    /// </summary>
    public class PassengerView
    {
        /// <summary>
        /// Идентификатор пассажира
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ФИО пассажира
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Паспортные данные пассажира
        /// </summary>
        public string PassportData { get; set; }

        /// <summary>
        /// Сохранена ли в БД (чтобы не сохранять заново)
        /// </summary>
        public bool IsSavedInDb { get; set; }

        public PassengerView()
        {
        }

        public PassengerView(Guid userId, string name, string passportData)
        {
            Id = Guid.Empty;
            Name = name;
            PassportData = passportData;
            IsSavedInDb = false;
        }

        public PassengerView(Passenger passenger)
        {
            Id = passenger.Id;
            Name = passenger.Name;
            PassportData = passenger.PassportData;
            IsSavedInDb = true;
        }

        public static List<PassengerView> FromPassengerList(List<Passenger> passenger)
        {
            return passenger.Select(passenger => new PassengerView(passenger)).ToList();
        }

        public KeyValuePair<Passenger, bool> ToPassenger(Guid userId)
        {
            var passenger = new Passenger
            {
                Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
                UserId = userId,
                Name = Name,
                PassportData = PassportData
            };
            return new KeyValuePair<Passenger, bool>(passenger, IsSavedInDb);
        }
    }
}

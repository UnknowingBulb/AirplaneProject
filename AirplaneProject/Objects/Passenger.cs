namespace AirplaneProject.Objects
{
    /// <summary>
    /// Пассажир
    /// </summary>
    public class Passenger
    {
        /// <summary>
        /// Идентификатор пассажира
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор автора пассажира
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Данные автора пассажира
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// ФИО пассажира
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Паспортные данные пассажира
        /// </summary>
        public string PassportData { get; set; }
    }
}

namespace AiplaneProject.Objects
{
    /// <summary>
    /// Пассажир
    /// </summary>
    public class Passenger
    {
        /// <summary>
        /// Идентификатор пассажира
        /// </summary>
        public Guid Guid;
        /// <summary>
        /// Идентификатор автора пассажира
        /// </summary>
        public Guid AuthorUserGuid;
        /// <summary>
        /// ФИО пассажира
        /// </summary>
        public string Name;
        /// <summary>
        /// Паспортные данные пассажира
        /// </summary>
        public string PassportData;
    }
}

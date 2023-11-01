using System.Security.Cryptography.X509Certificates;

namespace AiplaneProject.Database
{
    /// <summary>
    /// Рейс
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Идентификатор рейса
        /// </summary>
        public Guid Guid;
        /// <summary>
        /// Дата-время отправления
        /// </summary>
        public DateTime DepartureDateTime;
        /// <summary>
        /// Общее количество мест на рейс
        /// </summary>
        public int SeatingCapacity;
    }
}

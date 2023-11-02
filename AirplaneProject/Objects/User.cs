namespace AiplaneProject.Objects
{
    /// <summary>
    /// Пользователь сайта
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Является ли сотрудником
        /// </summary>
        public bool IsEmployee { get; set; }
    }
}

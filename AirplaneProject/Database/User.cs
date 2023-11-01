namespace AiplaneProject.Database
{
    /// <summary>
    /// Пользователь сайта
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Guid { get; set; }
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
    }
}

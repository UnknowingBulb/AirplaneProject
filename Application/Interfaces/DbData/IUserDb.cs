using AirplaneProject.Domain.Entities;

namespace AirplaneProject.Application.Interfaces.DbData
{
    /// <summary>
    /// Работа с БД по пользователям (клиентам/сотрудникам)
    /// </summary>
    public interface IUserDb
    {
        /// <summary>
        /// Получить пользователя
        /// </summary>
        public ValueTask<User?> GetAsync(Guid id);

        /// <summary>
        /// Получить пользователя
        /// </summary>
        public ValueTask<User?> GetAsync(string id);

        /// <summary>
        /// Получить пользователя с указанным логином
        /// </summary>
        public Task<User?> GetByLoginAsync(string login);

        /// <summary>
        /// Сохранить пользователя
        /// </summary>
        public Task SaveAsync(User user);

        /// <summary>
        /// Возвращает есть ли в БД пользователи с таким логином
        /// </summary>
        public Task<bool> IsAnyWithSameLoginAsync(string login);
    }
}

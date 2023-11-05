using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Работа с БД по пользователям (клиентам/сотрудникам)
    /// </summary>
    public class UserDb
    {
        private readonly ApplicationDbContext _dbContext;
        public UserDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        public ValueTask<User?> GetAsync(Guid id)
        {
            return _dbContext.User.FindAsync(id);
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        public ValueTask<User?> GetAsync(string id)
        {
            return GetAsync(new Guid(id));
        }

        /// <summary>
        /// Получить пользователя с указанным логином
        /// </summary>
        public Task<User?> GetByLoginAsync(string login)
        {
            return _dbContext.User.FirstOrDefaultAsync(user => user.Login == login);
        }

        /// <summary>
        /// Сохранить пользователя
        /// </summary>
        public Task SaveAsync(User user)
        {
            _dbContext.User.Add(user);
            return _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Возвращает есть ли в БД пользователи с таким логином
        /// </summary>
        public Task<bool> IsAnyWithSameLoginAsync(string login)
        {
            return _dbContext.User.AnyAsync(c => c.Login == login);
        }
    }
}

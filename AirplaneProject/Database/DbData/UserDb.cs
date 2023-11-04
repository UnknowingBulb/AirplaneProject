using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    public class UserDb
    {
        private readonly ApplicationDbContext _dbContext;
        public UserDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> Get(Guid id)
        {
            return _dbContext.User.FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<User?> Get(string id)
        {
            return Get(new Guid(id));
        }

        public Task<User?> GetByLogin(string login)
        {
            return _dbContext.User.FirstOrDefaultAsync(user => user.Login == login);
        }

        public Task Save(User user)
        {
            _dbContext.User.Add(user);
            return _dbContext.SaveChangesAsync();
        }

        public Task<bool> IsAnyWithSameLogin(string login)
        {
            return _dbContext.User.AnyAsync(c => c.Login == login);
        }
    }
}

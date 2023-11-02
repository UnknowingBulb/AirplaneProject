using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class UserDbContext : ApplicationContext
    {
        public UserDbContext()
            : base()
        {
        }

        public DbSet<AiplaneProject.Objects.User> Users => Set<AiplaneProject.Objects.User>();
    }
}

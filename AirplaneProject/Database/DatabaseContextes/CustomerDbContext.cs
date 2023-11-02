using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class CustomerDbContext : ApplicationContext
    {
        public CustomerDbContext()
            : base()
        {
        }

        public DbSet<AiplaneProject.Objects.CustomerUser> Customer => Set<AiplaneProject.Objects.CustomerUser>();
    }
}

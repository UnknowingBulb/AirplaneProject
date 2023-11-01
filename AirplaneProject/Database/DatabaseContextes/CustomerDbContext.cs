using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class CustomerDbContext : ApplicationContext
    {
        public CustomerDbContext()
            : base()
        {
        }

        public DbSet<AiplaneProject.Models.CustomerUser> Customer => Set<AiplaneProject.Models.CustomerUser>();
    }
}

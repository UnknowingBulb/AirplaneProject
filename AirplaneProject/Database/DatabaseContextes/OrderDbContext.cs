using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class OrderDbContext : ApplicationContext
    {
        public OrderDbContext()
            : base()
        {
        }

        public DbSet<Order> Order => Set<Order>();
    }
}

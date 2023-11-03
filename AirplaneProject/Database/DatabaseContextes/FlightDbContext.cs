using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class FlightDbContext : ApplicationContext
    {
        public FlightDbContext()
            : base()
        {
        }

        public DbSet<Flight> Flight => Set<Flight>();
    }
}

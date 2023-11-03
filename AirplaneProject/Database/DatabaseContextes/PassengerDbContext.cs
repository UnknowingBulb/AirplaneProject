using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class PassengerDbContext : ApplicationContext
    {
        public PassengerDbContext()
            : base()
        {
        }

        public DbSet<Passenger> Passenger => Set<Passenger>();
    }
}

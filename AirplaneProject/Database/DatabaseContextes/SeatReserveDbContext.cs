using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class SeatReserveDbContext : ApplicationContext
    {
        public SeatReserveDbContext()
            : base()
        {
        }

        public DbSet<SeatReserve> SeatReserve => Set<SeatReserve>();
    }
}

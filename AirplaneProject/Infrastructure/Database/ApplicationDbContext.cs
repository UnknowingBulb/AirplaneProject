using AirplaneProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Order> Order => Set<Order>();
        public virtual DbSet<Flight> Flight => Set<Flight>();
        public virtual DbSet<Passenger> Passenger => Set<Passenger>();
        public virtual DbSet<SeatReserve> SeatReserve => Set<SeatReserve>();
        public virtual DbSet<User> User => Set<User>();
    }
}
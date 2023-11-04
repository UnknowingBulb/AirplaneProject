using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Order => Set<Order>();
        public DbSet<Flight> Flight => Set<Flight>();
        public DbSet<Passenger> Passenger => Set<Passenger>();
        public DbSet<SeatReserve> SeatReserve => Set<SeatReserve>();
        public DbSet<User> User => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
        }
    }
}
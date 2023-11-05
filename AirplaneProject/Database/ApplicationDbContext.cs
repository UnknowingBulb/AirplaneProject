using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

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

            //TODO: remove logger
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new DebugLoggerProvider());
            //loggerFactory.AddProvider(new ConsoleLoggerProvider(new ConsoleLoggerOptions()));

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection")).UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging(true);
        }
    }
}
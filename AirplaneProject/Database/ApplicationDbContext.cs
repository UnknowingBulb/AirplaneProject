using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

namespace AirplaneProject.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<OrderModel> Order => Set<OrderModel>();
        public DbSet<FlightModel> Flight => Set<FlightModel>();
        public DbSet<PassengerModel> Passenger => Set<PassengerModel>();
        public DbSet<SeatReserveModel> SeatReserve => Set<SeatReserveModel>();
        public DbSet<UserModel> User => Set<UserModel>();

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
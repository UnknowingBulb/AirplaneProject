using AirplaneProject.Application.Interfaces;
using AirplaneProject.Application.Interfaces.DbData;
using AirplaneProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DbData
{
    /// <summary>
    /// Заполнить БД какими-нибудь данными
    /// </summary>
    public class SpawnDataDb : ISpawnDataDb
    {
        private readonly IApplicationDbContext _dbContext;
        public SpawnDataDb(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> IsNotEmptyAsync()
        {
            return _dbContext.User.AnyAsync();
        }

        public async Task<List<User>> AddUsersAsync()
        {
            var users = new List<User>
            {
                new User
                {
                    Login = "123456",
                    Password = "UGSRkj7cIwA9hpCY418iHg==;mGWcRmxq4otsp33JiHUFA/H687EkSOn/BpqH05gEMtc=",
                    Name = "Тестовый пользователь",
                    PhoneNumber = "+79999999",
                    IsEmployee = false
                },
                new User
                {
                    Login = "admin",
                    Password = "6+DOTwIyeRH+OLheGI3f1g==;zklQvxk3EYgidPkcsNvkgyTWGTYZkSSgpkQIs8n/XZg=",
                    Name = "admin",
                    PhoneNumber = "",
                    IsEmployee = true
                },
            };

            await _dbContext.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();
            return users;
        }

        public async Task<Passenger> AddPassengerAsync(User user)
        {
            var passenger = new Passenger
            {
                UserId = user.Id,
                Name = user.Name,
                PassportData = "1234 123456"
            };

            await _dbContext.AddAsync(passenger);
            await _dbContext.SaveChangesAsync();
            return passenger;
        }

        public async Task<List<Flight>> AddFlightsAsync()
        {
            var flights = new List<Flight>
            {
                new Flight
                {
                    DepartureDateTime = DateTime.UtcNow.AddDays(-3),
                    DepartureLocation = "Владивосток",
                    DestinationLocation = "Москва",
                    SeatingCapacity = 4,
                    Price = 40000,
                },
                new Flight
                {
                    DepartureDateTime = DateTime.UtcNow.AddDays(3),
                    DepartureLocation = "Владивосток",
                    DestinationLocation = "Москва",
                    SeatingCapacity = 4,
                    Price = 40300,
                },
                new Flight
                {
                    DepartureDateTime = DateTime.UtcNow.AddDays(5),
                    DepartureLocation = "Москва",
                    DestinationLocation = "Владивосток",
                    SeatingCapacity = 44,
                    Price = 40500,
                },
            };

            await _dbContext.AddRangeAsync(flights);
            await _dbContext.SaveChangesAsync();

            return flights;
        }
        public async Task AddOrdersAsync(Guid userId, Guid flightId, Guid passengerId)
        {
            var order = new Order
            {
                FlightId = flightId,
                UserId = userId,
                SeatReserves = new List<SeatReserve>
                {
                    new SeatReserve
                    {
                        PassengerId = passengerId,
                        SeatNumber = 1
                    },
                },
                Price = 100,
                IsActive = true,
            };

            await _dbContext.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}

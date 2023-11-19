using AirplaneProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AirplaneProject.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Order> Order { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<SeatReserve> SeatReserve { get; set; }
        public DbSet<User> User { get; set; }
        Task<int> SaveChangesAsync();
        Task AddRangeAsync(IEnumerable<object> entities);
        ValueTask<EntityEntry> AddAsync(object entity);
    }
}
using AirplaneProject.Application.Interfaces;
using AirplaneProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AirplaneProject.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Order> Order { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<SeatReserve> SeatReserve { get; set; }
        public DbSet<User> User { get; set; }

        public ValueTask<EntityEntry> AddAsync(object entity) => base.AddAsync(entity);

        public Task AddRangeAsync(IEnumerable<object> entities) => base.AddRangeAsync(entities);

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
    }
}
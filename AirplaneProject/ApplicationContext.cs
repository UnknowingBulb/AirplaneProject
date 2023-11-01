using AiplaneProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ClientUser> Users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=airplane-project;Username=admin;Password=admin");
        }
    }
}
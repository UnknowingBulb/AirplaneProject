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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
        }
    }
}
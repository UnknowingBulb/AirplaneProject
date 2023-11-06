﻿using AirplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Order> Order => Set<Order>();
        public virtual DbSet<Flight> Flight => Set<Flight>();
        public virtual DbSet<Passenger> Passenger => Set<Passenger>();
        public virtual DbSet<SeatReserve> SeatReserve => Set<SeatReserve>();
        public virtual DbSet<User> User => Set<User>();

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
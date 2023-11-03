﻿using AiplaneProject.Objects;
using Microsoft.EntityFrameworkCore;

namespace AirplaneProject.Database.DatabaseContextes
{
    public class UserDbContext : ApplicationContext
    {
        public UserDbContext()
            : base()
        {
        }

        public DbSet<User> User => Set<User>();
    }
}

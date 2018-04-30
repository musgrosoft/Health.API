using System;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace HealthAPI.Unit.Tests
{
    public class FakeLocalContext : HealthContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "test");
            }
        }
        
        
    }
}
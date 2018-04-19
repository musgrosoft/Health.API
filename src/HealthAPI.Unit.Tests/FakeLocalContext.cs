using System;
using HealthAPI.Models;
using Microsoft.EntityFrameworkCore;

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
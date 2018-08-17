using System;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Utils;

namespace HealthAPI.Unit.Tests
{
    public class FakeLocalContext : HealthContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            }
        }


        //public FakeLocalContext() : base(new Config())
        //{
        //}

        public FakeLocalContext() : base(new DbContextOptions<HealthContext>())
        {
        }
    }
}
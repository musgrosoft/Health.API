using Microsoft.EntityFrameworkCore;
using Repositories;
using Utils;

namespace Repositories
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


        public FakeLocalContext() : base(new Config())
        {
        }
    }
}
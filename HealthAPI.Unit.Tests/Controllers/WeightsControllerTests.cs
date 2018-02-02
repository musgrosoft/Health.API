using System.Collections.Generic;
using System.Linq;
using HealthAPI.Controllers;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers
{
    public class WeightsControllerTests
    {
        [Fact]
        public void Test1()
        {
            var data = new List<Weight>
            {
                new Weight{Kg = 123},
                new Weight{Kg = 456},
                new Weight{Kg = 789},
            }.AsQueryable();

            var dbSet = new Mock<DbSet<Weights>>();

            dbSet.As<IQueryable<Weight>>().Setup(m => m.Provider).Returns(data.Provider);
            dbSet.As<IQueryable<Weight>>().Setup(m => m.Expression).Returns(data.Expression);
            dbSet.As<IQueryable<Weight>>().Setup(m => m.ElementType).Returns(data.ElementType);
            dbSet.As<IQueryable<Weight>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            
            var healthContxt = new Mock<IHealthContext>();
            healthContxt.Setup(x => x.Weights).Returns(dbSet.Object);

            var weightsControlller = new WeightsController(healthContxt.Object);

            var result = weightsControlller.Get();

            Assert.Equal(result.Count(), 3);
            Assert.True(result.Any(x=>x.Kg == 123) );

        }
    }


}
using Repositories.Health;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Repository.Unit.Tests
{
    public class HealthRepositoryTests
    {
        private HealthRepository _healthRepository;
        private FakeLocalContext _fakeLocalContext;

        public HealthRepositoryTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _healthRepository = new HealthRepository(_fakeLocalContext);
        }

        [Fact]
        public void ShoudlSaveWeight()
        {
            var weight = new Weight { Kg = 123};

            _healthRepository.Insert(weight);

            var weights = _fakeLocalContext.Weights;

            Assert.Contains(weight, weights);
        }
    }
}

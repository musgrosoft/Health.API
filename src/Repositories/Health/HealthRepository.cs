using System.Threading.Tasks;
using Repositories.Models;
using Utils;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        Task<Maybe<Weight>> FindAsync(Weight weight);
        void Insert(Weight weight);
        void Update(Weight existingWeight, Weight newWeight);
    }

    public class HealthRepository : IHealthRepository
    {
        private readonly HealthContext _healthContext;

        public HealthRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        public void Insert(Weight weight)
        {
            _healthContext.Add(weight);
            _healthContext.SaveChanges();
        }

        public void Update(Weight existingWeight, Weight newWeight)
        {
            //check if datetimes are equal ???

            existingWeight.Kg = newWeight.Kg;
            existingWeight.FatRatioPercentage = newWeight.FatRatioPercentage;

            _healthContext.SaveChanges();
        }


        public async Task<Maybe<Weight>> FindAsync(Weight weight)
        {
            var existingWeight = await _healthContext.Weights.FindAsync(weight.DateTime);

            return Maybe<Weight>.CreateFrom(existingWeight);

            //if (existingWeight != null)
            //{
            //    return Maybe<Weight>.Some(existingWeight);
            //}
            //else
            //{
            //    return Maybe<Weight>.None;
            //}

            
        }
    }
}
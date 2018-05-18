using System.Threading.Tasks;
using Repositories.Models;
using Utils;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        Task<Maybe<Weight>> FindWeightAsync(Weight weight);
    }

    public class HealthRepository : IHealthRepository
    {
        private readonly HealthContext _healthContext;

        public HealthRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        public async Task<Maybe<Weight>> FindWeightAsync(Weight weight)
        {
            var existingWeight = await _healthContext.Weights.FindAsync(weight.DateTime);



            if (existingWeight != null)
            {
                return Maybe<Weight>.Some(existingWeight);
            }
            else
            {
                return Maybe<Weight>.None;
            }

            
        }
    }
}
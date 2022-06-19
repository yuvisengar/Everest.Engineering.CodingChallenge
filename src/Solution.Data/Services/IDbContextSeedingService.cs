using Everest.Engineering.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.Data.Services
{
    public interface IDbContextSeedingService
    {
        Task SeedData(IEnumerable<Offer> offers);
    }
}

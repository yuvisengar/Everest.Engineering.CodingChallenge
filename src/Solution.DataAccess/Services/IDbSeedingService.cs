using Everest.Engineering.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.DataAccess.Services
{
    public interface IDbSeedingService
    {
        Task SeedData(IEnumerable<Offer> offers);
    }
}

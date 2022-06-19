using Everest.Engineering.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.Business.Sevices.DataSeeder
{
    public interface IDataSeedingService
    {
        Task SeedData(IEnumerable<Offer> offers);
    }
}

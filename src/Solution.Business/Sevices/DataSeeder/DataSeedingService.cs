using Everest.Engineering.Data.Models;
using Everest.Engineering.DataAccess.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.Business.Sevices.DataSeeder
{
    public class DataSeedingService: IDataSeedingService
    {
        private readonly IDbSeedingService dbSeedingService;

        public DataSeedingService(IDbSeedingService dbSeedingService)
        {
            this.dbSeedingService = dbSeedingService;
        }

        public async Task SeedData(IEnumerable<Offer> offers)
        {
            await dbSeedingService.SeedData(offers);
        }
    }
}

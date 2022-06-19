using Everest.Engineering.Data.Models;
using Everest.Engineering.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.DataAccess.Services
{
    public class DbSeedingService: IDbSeedingService
    {
        private readonly IDbContextSeedingService dbContextSeedingService;

        public DbSeedingService(IDbContextSeedingService dbContextSeedingService)
        {
            this.dbContextSeedingService = dbContextSeedingService;
        }

        public async Task SeedData(IEnumerable<Offer> offers) 
        {
            await dbContextSeedingService.SeedData(offers);
        }
    }
}

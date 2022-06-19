using Everest.Engineering.Data.Abstractions;
using Everest.Engineering.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.Data.Services
{
    public class DbContextSeedingService : IDbContextSeedingService
    {
        private readonly IDataRepository<Offer> offerRepository;

        public DbContextSeedingService(IDataRepository<Offer> offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public async Task SeedData(IEnumerable<Offer> offers)
        {
            await offerRepository.Clean();
            await offerRepository.AddMany(offers);
        }
    }
}

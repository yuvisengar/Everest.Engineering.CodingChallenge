using Everest.Engineering.Data.Abstractions;
using Everest.Engineering.Data.DbContexts.Abstractions;
using Everest.Engineering.Data.Models;

namespace Everest.Engineering.DataAccess.Repositories
{
    public class OffersRepository : DataRepository<Offer>
    {   
        public OffersRepository(IDbContext<Offer> dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

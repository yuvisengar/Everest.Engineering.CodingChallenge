using Everest.Engineering.Data.DbContexts.Abstractions;
using Everest.Engineering.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Everest.Engineering.Data.DbContexts
{
    public class OffersDbContext: DbContext<Offer>
    {
        public OffersDbContext()
        {
            data = new List<Offer>();
        }

        public OffersDbContext(IEnumerable<Offer> offers)
        {
            data = offers.ToList();
        }
    }
}

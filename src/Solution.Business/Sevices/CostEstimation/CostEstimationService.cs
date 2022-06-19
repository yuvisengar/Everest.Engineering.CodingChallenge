using Everest.Engineering.Business.Models;
using Everest.Engineering.Data.Abstractions;
using Everest.Engineering.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.Engineering.Business.Sevices.CostEstimation
{
    public class CostEstimationService : ICostEstimationService
    {
        private readonly IDataRepository<Offer> offerRepository;

        public CostEstimationService(IDataRepository<Offer> offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public async Task<CostEstimateOutput> EstimateCost(CostEstimateInput input)
        {
            ValidateInput(input);
            var output = new CostEstimateOutput
            {
                PackageOutputs = new List<PackageCostOutput>()
            };

            foreach (var package in input.Packages)
            {
                var packageOutput = new PackageCostOutput
                {
                    Id = package.Id
                };
                var deliveryCost = GetDeliveryCost(input.BaseDeliveryCost, package);
                packageOutput.Discount = await GetDiscount(package, deliveryCost);
                packageOutput.Cost = deliveryCost - packageOutput.Discount;
                output.PackageOutputs.Add(packageOutput);
            }

            return output;
        }

        private static void ValidateInput(CostEstimateInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.NumberOfPackages < 1)
                throw new ArgumentException($"{nameof(input.NumberOfPackages)} should be more than 0");

            if (input.Packages is null)
                throw new ArgumentNullException(nameof(input.Packages));

            if (input.Packages.Count < 1)
                throw new ArgumentException($"{nameof(input.Packages)} should be more than 0");

            if (input.Packages.Any(x => x.Weight < 1))
                throw new ArgumentException($"{ nameof(PackageCostInput.Weight)} should be more than 0");

            if (input.Packages.Any(x => x.Distance < 1))
                throw new ArgumentException($"{ nameof(PackageCostInput.Distance)} should be more than 0");
        }

        private int GetDeliveryCost(int baseDeliveryCost, PackageCostInput package)
        {
            return baseDeliveryCost + (package.Weight * 10) + (package.Distance * 5);
        }

        private async Task<int> GetDiscount(PackageCostInput package, int deliveryCost)
        {
            var offer = (await offerRepository.GetAll()).SingleOrDefault(x => x.Name == package.OfferCode);
            if (offer is null)
            {
                return 0;
            }

            if (!IsWithinRange(package.Distance, offer.Criteria.Distance))
            {
                return 0;
            }

            if (!IsWithinRange(package.Weight, offer.Criteria.Weight))
            {
                return 0;
            }

            return (deliveryCost / 100) * offer.DiscountPercentage;
        }

        private bool IsWithinRange(int value, NumericalRange<int> range)
        {
            if (value >= range.Minimum && value <= range.Maximum)
            {
                return true;
            }

            return false;
        }
    }
}

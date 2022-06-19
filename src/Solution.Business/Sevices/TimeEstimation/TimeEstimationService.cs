using Everest.Engineering.Business.Models;
using Everest.Engineering.Business.Sevices.CostEstimation;
using Everest.Engineering.Business.Sevices.TimeEstimation;
using Everest.Engineering.Business.Utilities;
using Everest.Engineering.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.Engineering.Business.Sevices
{
    public class TimeEstimationService : ITimeEstimationService
    {
        private readonly ICostEstimationService costEstimationService;
        private readonly IPowerSubSetCalculator powerSubSetCalculator;
        private readonly IAppConfigurationProvider appConfigurationProvider;

        public TimeEstimationService(
            ICostEstimationService costEstimationService,
            IPowerSubSetCalculator powerSubSetCalculator,
            IAppConfigurationProvider appConfigurationProvider)
        {
            this.costEstimationService = costEstimationService;
            this.powerSubSetCalculator = powerSubSetCalculator;
            this.appConfigurationProvider = appConfigurationProvider;
        }

        public async Task<TimeEstimateOutput> EstimateCostAndTime(TimeEstimateInput input)
        {
            ValidateInput(input);
            var estimates = await GetCostEstimates(input);
            var vehicles = InitVehicles(input);
            while (AnyPackageLeftForDelivery(input))
            {
                var freeVehicle = GetFreeVehicles(vehicles);
                var packagesToDispatch = GetPackagesToDispatchThisIteration(input);
                var maxDistance = 0;
                foreach (var pkgToDeliver in packagesToDispatch)
                {
                    maxDistance = Math.Max(maxDistance, pkgToDeliver.Distance);
                    var pkg = estimates.PackageOutputs.First(x => x.Id == pkgToDeliver.Id);
                    pkg.DeliveryTime = TruncateTo2DecimalPlaces(freeVehicle.TimeTraveled + GetTime(pkgToDeliver.Distance, input.Vehicles.MaxSpeed));
                    pkgToDeliver.IsDelivered = true;
                }

                freeVehicle.TimeToBeFree = GetTime(maxDistance * 2, input.Vehicles.MaxSpeed);
            }

            return estimates;
        }

        private double GetTime(int distance, int speed)
        {
            return TruncateTo2DecimalPlaces((double)distance/speed);
        }

        private void ValidateInput(TimeEstimateInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.NumberOfPackages < 1)
                throw new ArgumentException($"{nameof(input.NumberOfPackages)} should be more than 0");

            if (input.Vehicles is null)
                throw new ArgumentNullException(nameof(input.Vehicles));

            if (input.Vehicles.TotalCount < 1)
                throw new ArgumentException($"{nameof(input.Vehicles.TotalCount)} should be more than 0");

            if (input.Vehicles.MaxSpeed < 1)
                throw new ArgumentException($"{nameof(input.Vehicles.MaxSpeed)} should be more than 0");

            if (input.Vehicles.MaxWeightCapacity < 1)
                throw new ArgumentException($"{nameof(input.Vehicles.MaxWeightCapacity)} should be more than 0");

            if (input.Packages is null)
                throw new ArgumentNullException(nameof(input.Packages));

            if (input.Packages.Count < 1)
                throw new ArgumentException($"{nameof(input.Packages)} should be more than 0");

            if (input.Packages.Any(x => x.Weight < 1))
                throw new ArgumentException($"{ nameof(PackageCostInput.Weight)} should be more than 0");

            if (input.Packages.Any(x => x.Distance < 1))
                throw new ArgumentException($"{ nameof(PackageCostInput.Distance)} should be more than 0");
        }

        private Vehicle GetFreeVehicles(List<Vehicle> vehicles)
        {
            var freeVehicle = vehicles.Where(x => x.IsFree).FirstOrDefault();
            if (freeVehicle is null)
            {
                Log("No Free Vehicle. Incrementing Time line.");
                freeVehicle = vehicles.OrderBy(x => x.TimeToBeFree).First();
                foreach (var v in vehicles)
                {
                    v.TimeTraveled += freeVehicle.TimeToBeFree;
                    v.TimeToBeFree -= freeVehicle.TimeToBeFree;
                }
            }

            Log("Next Free Vehicle - " + freeVehicle.Id);
            return freeVehicle;
        }

        private double TruncateTo2DecimalPlaces(double v)
        {
            var x = Math.Truncate(100 * v) / 100;
            return x;
        }

        private List<Vehicle> InitVehicles(TimeEstimateInput input)
        {
            var vehicles = new List<Vehicle>();
            for (int i = 1; i <= input.Vehicles.TotalCount; i++)
            {
                vehicles.Add(new Vehicle() { Id = i });
            }

            return vehicles;
        }

        private List<PackageCostInput> GetPackagesToDispatchThisIteration(TimeEstimateInput input)
        {
            var weightsArray = input.Packages.Where(x => !x.IsDelivered).Select(x => x.Weight).ToArray();
            var selectedPackageWeights = powerSubSetCalculator.GetMaxSubsetLessThan(weightsArray, input.Vehicles.MaxWeightCapacity + 1);
            var packagesToDispatch = new List<PackageCostInput>();
            foreach (var pkgWeight in selectedPackageWeights)
            {
                var packagesWithSameWeight = input.Packages.Where(x => x.Weight == pkgWeight && !x.IsDelivered).ToList();
                if (packagesWithSameWeight.Count() == 1)
                {
                    packagesToDispatch.Add(packagesWithSameWeight[0]);
                    packagesWithSameWeight[0].IsAllreadySelected = true;
                }
                else
                {
                    var pkg = packagesWithSameWeight.Where(x => !x.IsAllreadySelected).OrderBy(x => x.Distance).First();
                    packagesToDispatch.Add(pkg);
                    pkg.IsAllreadySelected = true;
                }
            }

            var sortedByWeight = packagesToDispatch.OrderBy(x => x.Distance).ToList();
            var pcks = "";
            foreach (var item in packagesToDispatch)
            {
                pcks += item.Id + " ";
            }
            Log("Got packages to be dispatched. They are - " + pcks);

            return sortedByWeight;
        }

        private bool AnyPackageLeftForDelivery(TimeEstimateInput input)
        {
            return input.Packages.Exists(x => !x.IsDelivered);
        }

        private async Task<TimeEstimateOutput> GetCostEstimates(TimeEstimateInput input)
        {
            var costEstimates = await costEstimationService.EstimateCost(input);
            var costandTimeEstimates = new TimeEstimateOutput() { PackageOutputs = new List<PackageTimeOutput>() };
            costEstimates.PackageOutputs.ForEach(x => costandTimeEstimates.PackageOutputs.Add(new PackageTimeOutput(x)));
            return costandTimeEstimates;
        }

        private void Log(string message)
        {
            if (appConfigurationProvider.IsLoggingEnabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}

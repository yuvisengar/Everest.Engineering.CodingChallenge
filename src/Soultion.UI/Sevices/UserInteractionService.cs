using Everest.Engineering.Business.Models;
using Everest.Engineering.Common.Configuration;
using Soultion.UI.Operations.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everest.Engineering.UI.Services
{
    public class UserInteractionService : IUserInteractionService
    {
        private readonly IRegularConsoleOperations console;
        private readonly IAppConfigurationProvider appConfig;

        public UserInteractionService(IRegularConsoleOperations console,
            IAppConfigurationProvider appConfig)
        {
            this.console = console;
            this.appConfig = appConfig;
        }

        public async Task<CostEstimateInput> GetCostEstimateInput()
        {
            if (appConfig.RunSampleTestCase)
                return appConfig.GetTestDataForCostEstimation();

            var line = await console.ReadLine();
            var splitted = line.Split();
            var input = new CostEstimateInput
            {
                BaseDeliveryCost = int.Parse(splitted[0]),
                NumberOfPackages = int.Parse(splitted[1]),
                Packages = new List<PackageCostInput>()
            };
            for (int i = 0; i < input.NumberOfPackages; i++)
            {
                line = await console.ReadLine();
                splitted = line.Split();
                var pkg = new PackageCostInput
                {
                    Id = splitted[0],
                    Weight = int.Parse(splitted[1]),
                    Distance = int.Parse(splitted[2]),
                    OfferCode = splitted[3]
                };

                input.Packages.Add(pkg);
            }

            return input;
        }

        public async Task<TimeEstimateInput> GetCostAndTimeEstimateInput()
        {
            if (appConfig.RunSampleTestCase)
                return appConfig.GetTestDataForTimeEstimation();

            var line = await console.ReadLine();
            var splitted = line.Split();
            var input = new TimeEstimateInput
            {
                BaseDeliveryCost = int.Parse(splitted[0]),
                NumberOfPackages = int.Parse(splitted[1]),
                Packages = new List<PackageCostInput>()
            };

            for (int i = 0; i < input.NumberOfPackages; i++)
            {
                line = await console.ReadLine();
                splitted = line.Split();
                var pkg = new PackageCostInput
                {
                    Id = splitted[0],
                    Weight = int.Parse(splitted[1]),
                    Distance = int.Parse(splitted[2]),
                    OfferCode = splitted[3]
                };
                input.Packages.Add(pkg);
            }

            line = await console.ReadLine();
            splitted = line.Split();
            input.Vehicles = new VehiclesInput
            {
                TotalCount = int.Parse(splitted[0]),
                MaxSpeed = int.Parse(splitted[1]),
                MaxWeightCapacity = int.Parse(splitted[2])
            };

            return input;
        }

        public async Task PrintCostEstimateOutput(CostEstimateOutput costEstimateOutput)
        {
            foreach (var pkg in costEstimateOutput.PackageOutputs)
            {
                await console.WriteLine($"{pkg.Id} {pkg.Discount} {pkg.Cost}");
            }
        }

        public async Task PrintCostAndTimeEstimateOutput(TimeEstimateOutput timeEstimateOutput)
        {
            foreach (var pkg in timeEstimateOutput.PackageOutputs)
            {
                await console.WriteLine($"{pkg.Id} {pkg.Discount} {pkg.Cost} {pkg.DeliveryTime}");
            }
        }
    }
}

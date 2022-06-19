using Everest.Engineering.Business.Sevices.CostEstimation;
using Everest.Engineering.Business.Sevices.DataSeeder;
using Everest.Engineering.Common.Configuration;
using Everest.Engineering.ConsoleApp.Executor.Abstractions;
using Everest.Engineering.UI.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Everest.Engineering.ConsoleApp.Executor.CostEstimation
{
    public class CostEstimationExecutor : IExecutor
    {
        private readonly IDataSeedingService dataSeedingService;
        private readonly IUserInteractionService userInteractionService;
        private readonly ICostEstimationService costEstimationService;
        private readonly IAppConfigurationProvider appConfigurationProvider;

        public CostEstimationExecutor(
            IDataSeedingService dataSeedingService,
            IUserInteractionService userInteractionService,
            ICostEstimationService costEstimationService,
            IAppConfigurationProvider appConfigurationProvider)
        {
            this.dataSeedingService = dataSeedingService;
            this.userInteractionService = userInteractionService;
            this.costEstimationService = costEstimationService;
            this.appConfigurationProvider = appConfigurationProvider;
        }

        public Task CancelOperationAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public async Task RunOperationsAsync(CancellationToken stoppingToken)
        {
            await dataSeedingService.SeedData(appConfigurationProvider.GetOffersToSeed());
            var input = await userInteractionService.GetCostEstimateInput();
            var output = await costEstimationService.EstimateCost(input);
            await userInteractionService.PrintCostEstimateOutput(output);
        }
    }
}

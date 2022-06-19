using Everest.Engineering.Business.Sevices.DataSeeder;
using Everest.Engineering.Business.Sevices.TimeEstimation;
using Everest.Engineering.Common.Configuration;
using Everest.Engineering.ConsoleApp.Executor.Abstractions;
using Everest.Engineering.UI.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Everest.Engineering.ConsoleApp.Executor.TimeEstimation
{
    public class TimeEstimationExecutor : IExecutor
    {
        private readonly IDataSeedingService dataSeedingService;
        private readonly IUserInteractionService userInteractionService;
        private readonly ITimeEstimationService timeEstimationService;
        private readonly IAppConfigurationProvider appConfigurationProvider;
        public TimeEstimationExecutor(
            IDataSeedingService dataSeedingService,
            IUserInteractionService userInteractionService,
            ITimeEstimationService timeEstimationService,
            IAppConfigurationProvider appConfigurationProvider)
        {
            this.dataSeedingService = dataSeedingService;
            this.userInteractionService = userInteractionService;
            this.timeEstimationService = timeEstimationService;
            this.appConfigurationProvider = appConfigurationProvider;
        }

        public Task CancelOperationAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task RunOperationsAsync(CancellationToken stoppingToken)
        {
            await dataSeedingService.SeedData(appConfigurationProvider.GetOffersToSeed());
            var input = await userInteractionService.GetCostAndTimeEstimateInput();
            var output = await timeEstimationService.EstimateCostAndTime(input);
            await userInteractionService.PrintCostAndTimeEstimateOutput(output);
        }
    }
}

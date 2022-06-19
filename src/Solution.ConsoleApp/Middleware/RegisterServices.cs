using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Everest.Engineering.Business.Sevices;
using Everest.Engineering.Business.Sevices.CostEstimation;
using Everest.Engineering.Business.Sevices.DataSeeder;
using Everest.Engineering.Business.Sevices.TimeEstimation;
using Everest.Engineering.Business.Utilities;
using Everest.Engineering.Common.Configuration;
using Everest.Engineering.ConsoleApp.Executor.Abstractions;
using Everest.Engineering.ConsoleApp.Executor.CostEstimation;
using Everest.Engineering.ConsoleApp.Executor.TimeEstimation;
using Everest.Engineering.Data.Abstractions;
using Everest.Engineering.Data.DbContexts;
using Everest.Engineering.Data.DbContexts.Abstractions;
using Everest.Engineering.Data.Models;
using Everest.Engineering.Data.Services;
using Everest.Engineering.DataAccess.Repositories;
using Everest.Engineering.DataAccess.Services;
using Everest.Engineering.UI.Services;
using Soultion.UI.Operations;
using Soultion.UI.Operations.Abstractions;

namespace Everest.Engineering.ConsoleApp
{
    /// <summary>
    /// Class Dependency Registrations.
    /// </summary>
    public static class RegisterServices
    {
        /// <summary>Configures the services.</summary>
        /// <param name="services"></param>
        /// <param name="hostContext"></param>
        /// <returns>IServiceCollection.</returns>
        public static void Configure(this IServiceCollection services, HostBuilderContext hostContext)
        {
            // Configuration
            services.AddSingleton<IAppConfigurationProvider, AppConfigurationProvider>();
            using var serviceProvider = services.BuildServiceProvider();
            var appConfigService = serviceProvider.GetRequiredService<IAppConfigurationProvider>();

            // Database Layer
            services.AddSingleton<IDbContext<Offer>, OffersDbContext>();

            //Data Seeding
            services.AddSingleton<IDbContextSeedingService, DbContextSeedingService>();
            services.AddSingleton<IDbSeedingService, DbSeedingService>();
            services.AddSingleton<IDataSeedingService, DataSeedingService>();

            // Data Access Layer
            services.AddSingleton<IDataRepository<Offer>, OffersRepository>();

            // User Interaction Layer
            services.AddSingleton<IRegularConsoleOperations, ConsoleOperations>();
            services.AddSingleton<IUserInteractionService, UserInteractionService>();
            services.AddSingleton<ICostEstimationService, CostEstimationService>();

            if (appConfigService.EstimationMode.Equals(Common.Models.EstimationModeType.Cost))
            {
                // Business Layer
                services.AddSingleton<IExecutor, CostEstimationExecutor>();
            }

            if (appConfigService.EstimationMode.Equals(Common.Models.EstimationModeType.Time))
            {
                // Business Layer
                services.AddSingleton<IPowerSubSetCalculator, RecursivePowerSubSetCalculator>();
                services.AddSingleton<ITimeEstimationService, TimeEstimationService>();
                services.AddSingleton<IExecutor, TimeEstimationExecutor>();
            }

            services.AddHostedService<Startup>();
        }
    }
}
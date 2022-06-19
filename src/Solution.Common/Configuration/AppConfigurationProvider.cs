using Microsoft.Extensions.Configuration;
using Everest.Engineering.Common.Models;
using System;
using System.Collections.Generic;
using Everest.Engineering.Data.Models;
using Everest.Engineering.Business.Models;

namespace Everest.Engineering.Common.Configuration
{
    /// <summary>Class AppConfigurationProvider.
    /// Implements the <see cref="IAppConfigurationProvider" /></summary>
    public class AppConfigurationProvider : IAppConfigurationProvider
    {
        private readonly IConfiguration configuration;

        public EstimationModeType EstimationMode { get; private set; }

        public bool RunSampleTestCase { get; private set; }

        public bool IsLoggingEnabled { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="T:ReservationSystem.ConsoleApp.Providers.Configuration.AppConfigurationProvider" /> class.</summary>
        /// <param name="configuration">The configuration.</param>
        public AppConfigurationProvider(IConfiguration configuration)
        {
            this.configuration = configuration;

            EstimationMode = EstimationModeType.Cost;
            var estimationMode = GetSettings("AppSettings:EstimationMode", "cost");
            if (estimationMode.Equals("time", StringComparison.InvariantCultureIgnoreCase))
                EstimationMode = EstimationModeType.Time;

            RunSampleTestCase = GetSettings("AppSettings:RunSampleTestCase", false);
            IsLoggingEnabled = GetSettings("AppSettings:IsLoggingEnabled", false);

        }

        private T GetSettings<T>(string key, T defaultValue)
        {
            try
            {
                return this.configuration.GetValue<T>(key, defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public IEnumerable<Offer> GetOffersToSeed()
        {
            return this.configuration.GetSection("DataToSeed:Offers").Get<List<Offer>>();
        }

        public CostEstimateInput GetTestDataForCostEstimation()
        {
            return this.configuration.GetSection("TestScenarios:CostEstimateInput").Get<CostEstimateInput>();
        }

        public TimeEstimateInput GetTestDataForTimeEstimation()
        {
            var s = this.configuration.GetSection("TestScenarios:TimeEstimateInput").Get<TimeEstimateInput>();
            return s;
        }
    }
}

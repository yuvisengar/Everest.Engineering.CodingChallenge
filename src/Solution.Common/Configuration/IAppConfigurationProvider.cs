using Everest.Engineering.Business.Models;
using Everest.Engineering.Common.Models;
using Everest.Engineering.Data.Models;
using System.Collections.Generic;

namespace Everest.Engineering.Common.Configuration
{
    public interface IAppConfigurationProvider
    {
        EstimationModeType EstimationMode { get; }
        bool RunSampleTestCase { get; }
        bool IsLoggingEnabled { get; }
        IEnumerable<Offer> GetOffersToSeed();
        CostEstimateInput GetTestDataForCostEstimation();
        TimeEstimateInput GetTestDataForTimeEstimation();
    }
}

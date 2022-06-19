using System.Collections.Generic;

namespace Everest.Engineering.Business.Models
{
    public class CostEstimateOutput
    {
        public List<PackageCostOutput> PackageOutputs { get; set; }

        public override string ToString()
        {
            return $"TotalPackages-{PackageOutputs.Count};";
        }
    }
}

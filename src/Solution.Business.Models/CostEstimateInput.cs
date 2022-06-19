using System.Collections.Generic;

namespace Everest.Engineering.Business.Models
{
    public class CostEstimateInput
    {
        public int BaseDeliveryCost { get; set; }
        public int NumberOfPackages { get; set; }
        public List<PackageCostInput> Packages { get; set; }

        public override string ToString()
        {
            return $"TotalPackages-{Packages.Count}; Cost-{BaseDeliveryCost};";
        }
    }
}

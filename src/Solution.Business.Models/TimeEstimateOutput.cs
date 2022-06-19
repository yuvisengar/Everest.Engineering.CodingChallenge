using System.Collections.Generic;

namespace Everest.Engineering.Business.Models
{
    public class TimeEstimateOutput
    {
        public List<PackageTimeOutput> PackageOutputs { get; set; }

        public override string ToString()
        {
            return $"Packages-{PackageOutputs.Count}";
        }
    }
}

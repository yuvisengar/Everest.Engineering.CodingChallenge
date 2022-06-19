using System.Collections.Generic;

namespace Everest.Engineering.Business.Utilities
{
    public interface IPowerSubSetCalculator
    {
        List<int> GetMaxSubsetLessThan(int[] weights, int maxSum);
    }
}

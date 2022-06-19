using System;
using System.Collections.Generic;
using System.Linq;

namespace Everest.Engineering.Business.Utilities
{
    public class RecursivePowerSubSetCalculator : IPowerSubSetCalculator
    {
        int arrSize;
        int maximumSetSum = -1;
        List<int> chosenPowerSet;

        public RecursivePowerSubSetCalculator()
        {
            Initialize();
        }

        private void Initialize()
        {
            arrSize = -1;
            maximumSetSum = -1;
            List<int> chosenPowerSet = new List<int>();
        }

        public List<int> GetMaxSubsetLessThan(int[] weights, int maxSum)
        {
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }

            if (weights.Count() == 0)
            {
                throw new ArgumentException($"{nameof(weights)} Cannot be empty.");
            }

            if (maxSum <= 0)
            {
                throw new ArgumentException($"{nameof(maxSum)} Cannot be less than 1.");
            }

            Initialize();
            RecursivelyCalculateMaximumSubset(weights, weights.Count(), 0, 0, new List<int>(), maxSum);
            return chosenPowerSet;
        }

        private void RecursivelyCalculateMaximumSubset(
            int[] weights,
            int numberOfPackages,
            int previousMaximum,
            int currentSum,
            List<int> packages,
            int maxSum)
        {
            if (currentSum > maxSum)
            {
                if (previousMaximum >= maximumSetSum)
                {
                    CompareSets(previousMaximum, packages.Count(), packages);
                }
                return;
            }

            if (numberOfPackages == 0)
            {
                if (currentSum <= maxSum && currentSum >= maximumSetSum)
                {
                    CompareSets(currentSum, packages.Count(), packages);
                }

                return;
            }

            List<int> tmp = new List<int>(packages);
            if (currentSum + weights[numberOfPackages - 1] <= maxSum)
            {
                tmp.Add(weights[numberOfPackages - 1]);
            }

            RecursivelyCalculateMaximumSubset(weights, numberOfPackages - 1, currentSum, currentSum + weights[numberOfPackages - 1], tmp, maxSum);
            RecursivelyCalculateMaximumSubset(weights, numberOfPackages - 1, previousMaximum, currentSum, new List<int>(packages), maxSum);
        }

        private void CompareSets(int previousMaximum, int currentSize, List<int> elements)
        {
            if (previousMaximum == maximumSetSum && currentSize > arrSize)
            {
                arrSize = currentSize;
                chosenPowerSet = elements;
            }

            else if (previousMaximum > maximumSetSum)
            {
                maximumSetSum = previousMaximum;
                arrSize = currentSize;
                chosenPowerSet = elements;
            }
        }
    }
}

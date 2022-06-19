using Everest.Engineering.Business.Utilities;
using FluentAssertions;
using System;
using Xunit;

namespace Everest.Highring.Business.UnitTests
{
    public class RecursivePowerSubSetCalculatorTests
    {
        [Fact]
        public void ThrowsException_IfArrayIsNull()
        {
            //Arrange
            int[] arr = null;
            int max = GetDefaultMaxSum();

            //Act
            IPowerSubSetCalculator target = new RecursivePowerSubSetCalculator();

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(() => target.GetMaxSubsetLessThan(arr, max));
            Assert.Equal("Value cannot be null. (Parameter 'weights')", exception.Message);
        }

        [Fact]
        public void ThrowsException_IfArrayIsEmpty()
        {
            //Arrange
            int[] arr = { };
            int max = GetDefaultMaxSum();

            //Act
            IPowerSubSetCalculator target = new RecursivePowerSubSetCalculator();

            //Assert
            var exception = Assert.Throws<ArgumentException>(() => target.GetMaxSubsetLessThan(arr, max));
            Assert.Equal("weights Cannot be empty.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ThrowsException_IfMaxSumIsLessThan1(int max)
        {
            //Arrange
            int[] arr = GetDefaultArray();

            //Act
            IPowerSubSetCalculator target = new RecursivePowerSubSetCalculator();

            //Assert
            var exception = Assert.Throws<ArgumentException>(() => target.GetMaxSubsetLessThan(arr, max));
            Assert.Equal("maxSum Cannot be less than 1.", exception.Message);
        }

        [Theory]
        [InlineData(new int[] { 50, 75, 175, 110, 155 }, 200, new int[] { 110, 75 })]
        [InlineData(new int[] { 50, 175, 155 }, 200, new int[] { 175 })]
        [InlineData(new int[] { 50, 155 }, 200, new int[] { 155 })]
        [InlineData(new int[] { 50 }, 200, new int[] { 50 })]
        public void Returns_ProperResults_Sample1(int[] inputArray, int maxSum, int[] expectedArray)
        {
            // Arranged in InlineData

            //Act
            IPowerSubSetCalculator target = new RecursivePowerSubSetCalculator();
            var result = target.GetMaxSubsetLessThan(inputArray, maxSum);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.NotEmpty(result);
            result.ToArray().Should().BeEquivalentTo(expectedArray);
        }

        [Theory]
        [InlineData(new int[] { 50, 50, 150, 99, 100 }, 200, new int[] { 100, 50, 50 })]
        [InlineData(new int[] { 150, 99 }, 200, new int[] { 150 })]
        [InlineData(new int[] { 100 }, 200, new int[] { 100 })]
        public void Returns_ProperResults_Sample2(int[] inputArray, int maxSum, int[] expectedArray)
        {
            // Arranged in InlineData

            //Act
            IPowerSubSetCalculator target = new RecursivePowerSubSetCalculator();
            var result = target.GetMaxSubsetLessThan(inputArray, maxSum);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.NotEmpty(result);
            result.ToArray().Should().BeEquivalentTo(expectedArray);
        }

        private static int GetDefaultMaxSum()
        {
            return 200;
        }

        private int[] GetDefaultArray()
        {
            return new int[] { 50, 75, 175, 110, 155 };
        }
    }
}

using Everest.Engineering.Business.Models;
using Everest.Engineering.Business.Sevices;
using Everest.Engineering.Business.Sevices.CostEstimation;
using Everest.Engineering.Business.Sevices.TimeEstimation;
using Everest.Engineering.Business.Utilities;
using Everest.Engineering.Common.Configuration;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Everest.Highring.Business.UnitTests
{
    public class TimeEstimaionServiceTests
    {
        private readonly ICostEstimationService costEstimationServiceDependency;
        private readonly IPowerSubSetCalculator powerSubSetCalculatorDependency;
        private readonly IAppConfigurationProvider appConfigurationProviderDependency;

        public TimeEstimaionServiceTests()
        {
            costEstimationServiceDependency = Substitute.For<ICostEstimationService>();
            powerSubSetCalculatorDependency = Substitute.For<IPowerSubSetCalculator>();
            appConfigurationProviderDependency = Substitute.For<IAppConfigurationProvider>();
            appConfigurationProviderDependency.IsLoggingEnabled.Returns(false);
        }

        [Fact]
        public async Task ThrowsException_If_Input_IsNull()
        {
            // Arrange
            TimeEstimateInput input = null;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal("input", exception.ParamName);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_NumberOfPackages_IsLessThan1(int numberOfPackages)
        {
            // Arrange
            var input = GetSampleInput1();
            input.NumberOfPackages = numberOfPackages;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal($"{nameof(input.NumberOfPackages)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_Weight_IsInValid(int weight)
        {
            // Arrange
            var input = GetSampleInput1();
            input.Packages.First().Weight = weight;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal($"{ nameof(PackageCostInput.Weight)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_Distance_IsInValid(int distance)
        {
            // Arrange
            var input = GetSampleInput1();
            input.Packages.First().Distance = distance;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal($"{ nameof(PackageCostInput.Distance)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_VehicleCount_IsInValid(int numberOfVehicles)
        {
            // Arrange
            var input = GetSampleInput1();
            input.Vehicles.TotalCount = numberOfVehicles;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal($"{ nameof(input.Vehicles.TotalCount)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_VehicleMaxWeightCapacity_IsInValid(int maxWeightCapacity)
        {
            // Arrange
            var input = GetSampleInput1();
            input.Vehicles.MaxWeightCapacity = maxWeightCapacity;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal($"{ nameof(input.Vehicles.MaxWeightCapacity)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_VehicleMaxSpeed_IsInValid(int maxSpeed)
        {
            // Arrange
            var input = GetSampleInput1();
            input.Vehicles.MaxSpeed = maxSpeed;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(()
                => GetTarget().EstimateCostAndTime(input));

            // Assert
            Assert.Equal($"{ nameof(input.Vehicles.MaxSpeed)} should be more than 0", exception.Message);
        }

        [Fact]
        public async Task ReturnsValid_TimeEstimation_Sample1()
        {
            // Arrange
            var input = GetSampleInput1();

            costEstimationServiceDependency.
                EstimateCost(Arg.Any<CostEstimateInput>()).
                Returns(Task.FromResult(GetSampleCostOutput1(input)));

            var maxWeightCapacity = input.Vehicles.MaxWeightCapacity + 1;
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 5), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 110, 75 });
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 3), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 175 });
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 2), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 155 });
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 1), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 50 });

            var result = await GetTarget().EstimateCostAndTime(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TimeEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageTimeOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageTimeOutput>();
            result.PackageOutputs.Should().HaveCount(5);

            result.PackageOutputs[0].Should().NotBeNull();
            result.PackageOutputs[0].Id.Should().Be("PKG1");
            result.PackageOutputs[0].Discount.Should().Be(0);
            result.PackageOutputs[0].Cost.Should().Be(750);
            result.PackageOutputs[0].DeliveryTime.Should().Be(3.99);

            result.PackageOutputs[1].Should().NotBeNull();
            result.PackageOutputs[1].Id.Should().Be("PKG2");
            result.PackageOutputs[1].Discount.Should().Be(0);
            result.PackageOutputs[1].Cost.Should().Be(1475);
            result.PackageOutputs[1].DeliveryTime.Should().Be(1.78);

            result.PackageOutputs[2].Should().NotBeNull();
            result.PackageOutputs[2].Id.Should().Be("PKG3");
            result.PackageOutputs[2].Discount.Should().Be(0);
            result.PackageOutputs[2].Cost.Should().Be(2350);
            result.PackageOutputs[2].DeliveryTime.Should().Be(1.42);

            result.PackageOutputs[3].Should().NotBeNull();
            result.PackageOutputs[3].Id.Should().Be("PKG4");
            result.PackageOutputs[3].Discount.Should().Be(105);
            result.PackageOutputs[3].Cost.Should().Be(1395);
            result.PackageOutputs[3].DeliveryTime.Should().Be(0.85);

            result.PackageOutputs[4].Should().NotBeNull();
            result.PackageOutputs[4].Id.Should().Be("PKG5");
            result.PackageOutputs[4].Discount.Should().Be(0);
            result.PackageOutputs[4].Cost.Should().Be(2125);
            result.PackageOutputs[4].DeliveryTime.Should().Be(4.2);
        }

        [Fact]
        public async Task ReturnsValid_TimeEstimation_Sample2()
        {
            // Arrange
            var input = GetSampleInput2();

            costEstimationServiceDependency.
                EstimateCost(Arg.Any<CostEstimateInput>()).
                Returns(Task.FromResult(GetSampleCostOutput2(input)));

            var maxWeightCapacity = input.Vehicles.MaxWeightCapacity + 1;
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 5), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 100, 50, 50 });
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 2), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 150 });
            powerSubSetCalculatorDependency.
                GetMaxSubsetLessThan(Arg.Is<int[]>(x => x.Length == 1), Arg.Is(maxWeightCapacity)).
                Returns(new List<int>() { 99 });

            var result = await GetTarget().EstimateCostAndTime(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TimeEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageTimeOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageTimeOutput>();
            result.PackageOutputs.Should().HaveCount(5);

            result.PackageOutputs[0].Should().NotBeNull();
            result.PackageOutputs[0].Id.Should().Be("P1");
            result.PackageOutputs[0].Discount.Should().Be(0);
            result.PackageOutputs[0].Cost.Should().Be(1100);
            result.PackageOutputs[0].DeliveryTime.Should().Be(1.42);

            result.PackageOutputs[1].Should().NotBeNull();
            result.PackageOutputs[1].Id.Should().Be("P2");
            result.PackageOutputs[1].Discount.Should().Be(0);
            result.PackageOutputs[1].Cost.Should().Be(1100);
            result.PackageOutputs[1].DeliveryTime.Should().Be(1.42);

            result.PackageOutputs[2].Should().NotBeNull();
            result.PackageOutputs[2].Id.Should().Be("P3");
            result.PackageOutputs[2].Discount.Should().Be(105);
            result.PackageOutputs[2].Cost.Should().Be(1995);
            result.PackageOutputs[2].DeliveryTime.Should().Be(4.26);

            result.PackageOutputs[3].Should().NotBeNull();
            result.PackageOutputs[3].Id.Should().Be("P4");
            result.PackageOutputs[3].Discount.Should().Be(75);
            result.PackageOutputs[3].Cost.Should().Be(1515);
            result.PackageOutputs[3].DeliveryTime.Should().Be(7.12);

            result.PackageOutputs[4].Should().NotBeNull();
            result.PackageOutputs[4].Id.Should().Be("P5");
            result.PackageOutputs[4].Discount.Should().Be(80);
            result.PackageOutputs[4].Cost.Should().Be(1520);
            result.PackageOutputs[4].DeliveryTime.Should().Be(1.42);
        }

        private TimeEstimateInput GetSampleInput2()
        {
            return new TimeEstimateInput
            {
                BaseDeliveryCost = 100,
                NumberOfPackages = 5,
                Packages = new List<PackageCostInput>()
                {
                    new PackageCostInput() { Id = "P1", Weight = 50, Distance = 100, OfferCode = "O1" },
                    new PackageCostInput() { Id = "P2", Weight = 50, Distance = 100, OfferCode = "O2" },
                    new PackageCostInput() { Id = "P3", Weight = 150, Distance = 100, OfferCode = "O3" },
                    new PackageCostInput() { Id = "P4", Weight = 99, Distance = 100, OfferCode = "O4" },
                    new PackageCostInput() { Id = "P5", Weight = 100, Distance = 100, OfferCode = "O5" }
                },
                Vehicles = new VehiclesInput()
                {
                    TotalCount = 1,
                    MaxSpeed = 70,
                    MaxWeightCapacity = 200
                }
            };
        }

        private CostEstimateOutput GetSampleCostOutput2(CostEstimateInput input)
        {
            return new CostEstimateOutput
            {
                PackageOutputs = new List<PackageCostOutput>
                {
                    new PackageCostOutput() { Id = "P1", Discount = 0, Cost = 1100 },
                    new PackageCostOutput() { Id = "P2", Discount = 0, Cost = 1100 },
                    new PackageCostOutput() { Id = "P3", Discount = 105, Cost = 1995 },
                    new PackageCostOutput() { Id = "P4", Discount = 75, Cost = 1515 },
                    new PackageCostOutput() { Id = "P5", Discount = 80, Cost = 1520 }
                }
            };
        }

        private TimeEstimateInput GetSampleInput1(int packagesInside = 5)
        {
            var input = new TimeEstimateInput
            {
                BaseDeliveryCost = 100,
                Vehicles = new VehiclesInput()
                {
                    MaxSpeed = 70,
                    TotalCount = 2,
                    MaxWeightCapacity = 200
                },

                NumberOfPackages = packagesInside,
                Packages = new List<PackageCostInput>()
            };

            if (packagesInside > 0)
                input.Packages.Add(new PackageCostInput() { Id = "PKG1", Weight = 50, Distance = 30, OfferCode = "OFR001" });
            if (packagesInside > 1)
                input.Packages.Add(new PackageCostInput() { Id = "PKG2", Weight = 75, Distance = 125, OfferCode = "OFR008" });
            if (packagesInside > 2)
                input.Packages.Add(new PackageCostInput() { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFR003" });
            if (packagesInside > 3)
                input.Packages.Add(new PackageCostInput() { Id = "PKG4", Weight = 110, Distance = 60, OfferCode = "OFR002" });
            if (packagesInside > 4)
                input.Packages.Add(new PackageCostInput() { Id = "PKG5", Weight = 155, Distance = 95, OfferCode = "NA" });

            if (packagesInside > 5)
            {
                for (int i = 6; i <= packagesInside; i++)
                {
                    input.Packages.Add(new PackageCostInput() { Id = $"PKG{i}", Weight = i * 5, Distance = i * 15, OfferCode = $"OFR00{i}" });
                }
            }

            return input;
        }

        private CostEstimateOutput GetSampleCostOutput1(CostEstimateInput input)
        {
            return new CostEstimateOutput
            {
                PackageOutputs = new List<PackageCostOutput>
                {
                    new PackageCostOutput() { Id = "PKG1", Discount = 0, Cost = 750 },
                    new PackageCostOutput() { Id = "PKG2", Discount = 0, Cost = 1475 },
                    new PackageCostOutput() { Id = "PKG3", Discount = 0, Cost = 2350 },
                    new PackageCostOutput() { Id = "PKG4", Discount = 105, Cost = 1395 },
                    new PackageCostOutput() { Id = "PKG5", Discount = 0, Cost = 2125 }
                }
            };
        }

        private ITimeEstimationService GetTarget()
        {
            return new TimeEstimationService(
                            costEstimationServiceDependency,
                            powerSubSetCalculatorDependency,
                            appConfigurationProviderDependency);
        }
    }
}

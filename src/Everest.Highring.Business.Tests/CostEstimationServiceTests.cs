using Everest.Engineering.Business.Models;
using Everest.Engineering.Business.Sevices.CostEstimation;
using Everest.Engineering.Data.Abstractions;
using Everest.Engineering.Data.Models;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Everest.Highring.Business.UnitTests
{
    public class CostEstimationServiceTests
    {
        private readonly IDataRepository<Offer> offerRepositoryDependency;

        public CostEstimationServiceTests()
        {
            offerRepositoryDependency = Substitute.For<IDataRepository<Offer>>();
        }

        [Fact]
        public async Task ThrowsException_If_Input_IsNull()
        {
            // Arrange
            CostEstimateInput input = null;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => target.EstimateCost(input));

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
            var input = GetSampleInput();
            input.NumberOfPackages = numberOfPackages;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => target.EstimateCost(input));

            // Assert
            Assert.Equal($"{nameof(input.NumberOfPackages)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public async Task ReturnsResult_If_NumberOfPackages_IsGreaterThan0(int numberOfPackages)
        {
            // Arrange
            var input = GetSampleInput();
            input.NumberOfPackages = numberOfPackages;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_Weight_IsInValid(int weight)
        {
            // Arrange
            var input = GetSampleInput();
            input.Packages.First().Weight = weight;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => target.EstimateCost(input));

            // Assert
            Assert.Equal($"{ nameof(PackageCostInput.Weight)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(int.MaxValue)]
        public async Task ReturnsResult_If_Weight_IsValid(int weight)
        {
            // Arrange
            var input = GetSampleInput();
            input.Packages.First().Weight = weight;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ThrowsException_If_Distance_IsInValid(int distance)
        {
            // Arrange
            var input = GetSampleInput();
            input.Packages.First().Distance = distance;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => target.EstimateCost(input));

            // Assert
            Assert.Equal($"{ nameof(PackageCostInput.Distance)} should be more than 0", exception.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(int.MaxValue)]
        public async Task ReturnsResult_If_Distance_IsValid(int distance)
        {
            // Arrange
            var input = GetSampleInput();
            input.Packages.First().Distance = distance;

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
        }

        [Fact]
        public async Task ReturnsValid_DeliveryCost()
        {
            // Arrange
            var input = GetSampleInput(1);

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
            result.PackageOutputs.First().Cost.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(1, 1, 15, 10, 100, 20, 40, 0)]
        [InlineData(9, 19, 15, 10, 100, 20, 40, 0)]
        [InlineData(10, 19, 15, 10, 100, 20, 40, 0)]
        [InlineData(11, 19, 15, 10, 100, 20, 40, 0)]
        [InlineData(99, 19, 15, 10, 100, 20, 40, 0)]
        [InlineData(100, 19, 15, 10, 100, 20, 40, 0)]
        [InlineData(101, 19, 15, 10, 100, 20, 40, 0)]
        [InlineData(9, 20, 15, 10, 100, 20, 40, 0)]
        [InlineData(9, 21, 15, 10, 100, 20, 40, 0)]
        [InlineData(9, 39, 15, 10, 100, 20, 40, 0)]
        [InlineData(9, 40, 15, 10, 100, 20, 40, 0)]
        [InlineData(9, 41, 15, 10, 100, 20, 40, 0)]
        [InlineData(10, 20, 15, 10, 100, 20, 40, 45)]
        [InlineData(11, 20, 15, 10, 100, 20, 40, 45)]
        [InlineData(10, 21, 15, 10, 100, 20, 40, 45)]
        [InlineData(11, 21, 15, 10, 100, 20, 40, 45)]
        [InlineData(20, 24, 15, 10, 100, 20, 40, 60)]
        [InlineData(40, 28, 15, 10, 100, 20, 40, 90)]
        [InlineData(60, 32, 15, 10, 100, 20, 40, 120)]
        [InlineData(80, 36, 15, 10, 100, 20, 40, 150)]
        [InlineData(99, 39, 15, 10, 100, 20, 40, 180)]
        [InlineData(100, 40, 15, 10, 100, 20, 40, 195)]
        [InlineData(101, 41, 15, 10, 100, 20, 40, 0)]
        public async Task Applies_ValidOffer(
            int weightOfPackage,
            int distanceOfPackage,
            int discountToAppliedByOffer,
            int MinWeightForDiscountToApply,
            int MaxWeightForDiscountToApply,
            int MinDistanceForDiscountToApply,
            int MaxDistanceForDiscountToApply,
            int expectedDiscount)
        {
            // Arrange
            var input = GetSampleInput(1);
            var inputPkg = input.Packages.First();
            inputPkg.Weight = weightOfPackage;
            inputPkg.Distance = distanceOfPackage;

            var offer = new Offer()
            {
                Name = "OFR001",

                DiscountPercentage = discountToAppliedByOffer,
                Criteria = new OfferCriteria()
                {
                    Distance = new NumericalRange<int>()
                    {
                        Minimum = MinDistanceForDiscountToApply,
                        Maximum = MaxDistanceForDiscountToApply
                    },
                    Weight = new NumericalRange<int>()
                    {
                        Minimum = MinWeightForDiscountToApply,
                        Maximum = MaxWeightForDiscountToApply
                    }
                }
            };

            input.Packages.First().OfferCode = offer.Name;
            offerRepositoryDependency.
                GetAll().
                Returns(Task.FromResult(GetEnumerable(new List<Offer> { offer })));

            // Act
            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
            result.PackageOutputs.First().Discount.Should().Be(expectedDiscount);
        }

        [Theory]
        [InlineData(1, 1, 15, 10, 100, 20, 40, 0, 115)]
        [InlineData(9, 19, 15, 10, 100, 20, 40, 0, 285)]
        [InlineData(10, 19, 15, 10, 100, 20, 40, 0, 295)]
        [InlineData(11, 19, 15, 10, 100, 20, 40, 0, 305)]
        [InlineData(99, 19, 15, 10, 100, 20, 40, 0, 1185)]
        [InlineData(100, 19, 15, 10, 100, 20, 40, 0, 1195)]
        [InlineData(101, 19, 15, 10, 100, 20, 40, 0, 1205)]
        [InlineData(9, 20, 15, 10, 100, 20, 40, 0, 290)]
        [InlineData(9, 21, 15, 10, 100, 20, 40, 0, 295)]
        [InlineData(9, 39, 15, 10, 100, 20, 40, 0, 385)]
        [InlineData(9, 40, 15, 10, 100, 20, 40, 0, 390)]
        [InlineData(9, 41, 15, 10, 100, 20, 40, 0, 395)]
        [InlineData(10, 20, 15, 10, 100, 20, 40, 45, 255)]
        [InlineData(11, 20, 15, 10, 100, 20, 40, 45, 265)]
        [InlineData(10, 21, 15, 10, 100, 20, 40, 45, 260)]
        [InlineData(11, 21, 15, 10, 100, 20, 40, 45, 270)]
        [InlineData(20, 24, 15, 10, 100, 20, 40, 60, 360)]
        [InlineData(40, 28, 15, 10, 100, 20, 40, 90, 550)]
        [InlineData(60, 32, 15, 10, 100, 20, 40, 120, 740)]
        [InlineData(80, 36, 15, 10, 100, 20, 40, 150, 930)]
        [InlineData(99, 39, 15, 10, 100, 20, 40, 180, 1105)]
        [InlineData(100, 40, 15, 10, 100, 20, 40, 195, 1105)]
        [InlineData(101, 41, 15, 10, 100, 20, 40, 0, 1315)]
        public async Task ReturnsValid_CostEstimation(
            int weightOfPackage,
            int distanceToTravel,
            int discountToAppliedByOffer,
            int MinWeightForDiscountToApply,
            int MaxWeightForDiscountToApply,
            int MinDistanceForDiscountToApply,
            int MaxDistanceForDiscountToApply,
            int expectedDiscount,
            int expectedCost)
        {
            // Arrange
            var input = GetSampleInput(1);
            var inputPkg = input.Packages.First();
            inputPkg.Weight = weightOfPackage;
            inputPkg.Distance = distanceToTravel;
            var offer = new Offer()
            {
                Name = "OFR001",

                DiscountPercentage = discountToAppliedByOffer,
                Criteria = new OfferCriteria()
                {
                    Distance = new NumericalRange<int>()
                    {
                        Minimum = MinDistanceForDiscountToApply,
                        Maximum = MaxDistanceForDiscountToApply
                    },
                    Weight = new NumericalRange<int>()
                    {
                        Minimum = MinWeightForDiscountToApply,
                        Maximum = MaxWeightForDiscountToApply
                    }
                }
            };

            input.Packages.First().OfferCode = offer.Name;
            offerRepositoryDependency.
                GetAll().
                Returns(Task.FromResult(GetEnumerable(new List<Offer> { offer })));

            // Act

            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
            var pkgOutput = result.PackageOutputs.First();
            pkgOutput.Discount.Should().Be(expectedDiscount);
            pkgOutput.Cost.Should().BeGreaterThan(0);
            pkgOutput.Cost.Should().Be(expectedCost);
        }

        private IEnumerable<T> GetEnumerable<T>(List<T> list)
        {
            return new List<T>(list);
        }

        [Fact]
        public async void EstimatesCostCorrectly_Sample1()
        {
            var input = new CostEstimateInput
            {
                BaseDeliveryCost = 100,
                NumberOfPackages = 3,
                Packages = new List<PackageCostInput>
                {
                    new PackageCostInput() { Id = "PKG1", Weight = 5, Distance = 5, OfferCode = "OFR001" },
                    new PackageCostInput() { Id = "PKG2", Weight = 15, Distance = 5, OfferCode = "OFR002" },
                    new PackageCostInput() { Id = "PKG3", Weight = 10, Distance = 100, OfferCode = "OFR003" }
                }
            };

            offerRepositoryDependency.
                GetAll().
                Returns(Task.FromResult(GetSampleOffers()));

            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
            result.PackageOutputs.Should().HaveCount(3);

            result.PackageOutputs[0].Should().NotBeNull();
            result.PackageOutputs[0].Id.Should().Be("PKG1");
            result.PackageOutputs[0].Discount.Should().Be(0);
            result.PackageOutputs[0].Cost.Should().Be(175);

            result.PackageOutputs[1].Should().NotBeNull();
            result.PackageOutputs[1].Id.Should().Be("PKG2");
            result.PackageOutputs[1].Discount.Should().Be(0);
            result.PackageOutputs[1].Cost.Should().Be(275);

            result.PackageOutputs[2].Should().NotBeNull();
            result.PackageOutputs[2].Id.Should().Be("PKG3");
            result.PackageOutputs[2].Discount.Should().Be(35);
            result.PackageOutputs[2].Cost.Should().Be(665);
        }

        [Fact]
        public async void EstimatesCostCorrectly_Sample2()
        {
            var input = new CostEstimateInput
            {
                BaseDeliveryCost = 100,
                NumberOfPackages = 5,
                Packages = new List<PackageCostInput>
                {
                    new PackageCostInput() { Id = "PKG1", Weight = 50, Distance = 30, OfferCode = "OFR001" },
                    new PackageCostInput() { Id = "PKG2", Weight = 75, Distance = 125, OfferCode = "OFR008" },
                    new PackageCostInput() { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFR003" },
                    new PackageCostInput() { Id = "PKG4", Weight = 110, Distance = 60, OfferCode = "OFR002" },
                    new PackageCostInput() { Id = "PKG5", Weight = 155, Distance = 95, OfferCode = "NA" }
                }
            };

            offerRepositoryDependency.
                GetAll().
                Returns(Task.FromResult(GetSampleOffers()));

            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
            result.PackageOutputs.Should().HaveCount(5);

            result.PackageOutputs[0].Should().NotBeNull();
            result.PackageOutputs[0].Id.Should().Be("PKG1");
            result.PackageOutputs[0].Discount.Should().Be(0);
            result.PackageOutputs[0].Cost.Should().Be(750);

            result.PackageOutputs[1].Should().NotBeNull();
            result.PackageOutputs[1].Id.Should().Be("PKG2");
            result.PackageOutputs[1].Discount.Should().Be(0);
            result.PackageOutputs[1].Cost.Should().Be(1475);

            result.PackageOutputs[2].Should().NotBeNull();
            result.PackageOutputs[2].Id.Should().Be("PKG3");
            result.PackageOutputs[2].Discount.Should().Be(0);
            result.PackageOutputs[2].Cost.Should().Be(2350);

            result.PackageOutputs[3].Should().NotBeNull();
            result.PackageOutputs[3].Id.Should().Be("PKG4");
            result.PackageOutputs[3].Discount.Should().Be(105);
            result.PackageOutputs[3].Cost.Should().Be(1395);

            result.PackageOutputs[4].Should().NotBeNull();
            result.PackageOutputs[4].Id.Should().Be("PKG5");
            result.PackageOutputs[4].Discount.Should().Be(0);
            result.PackageOutputs[4].Cost.Should().Be(2125);
        }

        [Fact]
        public async void EstimatesCostCorrectly_Sample3()
        {
            var input = new CostEstimateInput
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
            };

            offerRepositoryDependency.
                GetAll().
                Returns(Task.FromResult(GetSampleOffers2()));

            ICostEstimationService target = new CostEstimationService(offerRepositoryDependency);
            var result = await target.EstimateCost(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CostEstimateOutput>();
            result.PackageOutputs.Should().NotBeNull();
            result.PackageOutputs.Should().BeOfType<List<PackageCostOutput>>();
            result.PackageOutputs.Should().AllBeOfType<PackageCostOutput>();
            result.PackageOutputs.Should().HaveCount(5);

            result.PackageOutputs[0].Should().NotBeNull();
            result.PackageOutputs[0].Id.Should().Be("P1");
            result.PackageOutputs[0].Discount.Should().Be(0);
            result.PackageOutputs[0].Cost.Should().Be(1100);

            result.PackageOutputs[1].Should().NotBeNull();
            result.PackageOutputs[1].Id.Should().Be("P2");
            result.PackageOutputs[1].Discount.Should().Be(0);
            result.PackageOutputs[1].Cost.Should().Be(1100);

            result.PackageOutputs[2].Should().NotBeNull();
            result.PackageOutputs[2].Id.Should().Be("P3");
            result.PackageOutputs[2].Discount.Should().Be(105);
            result.PackageOutputs[2].Cost.Should().Be(1995);

            result.PackageOutputs[3].Should().NotBeNull();
            result.PackageOutputs[3].Id.Should().Be("P4");
            result.PackageOutputs[3].Discount.Should().Be(75);
            result.PackageOutputs[3].Cost.Should().Be(1515);

            result.PackageOutputs[4].Should().NotBeNull();
            result.PackageOutputs[4].Id.Should().Be("P5");
            result.PackageOutputs[4].Discount.Should().Be(80);
            result.PackageOutputs[4].Cost.Should().Be(1520);
        }

        public CostEstimateInput GetSampleInput(int packagesInside = 3)
        {
            var input = new CostEstimateInput();
            input.BaseDeliveryCost = 100;
            input.NumberOfPackages = packagesInside;
            input.Packages = new List<PackageCostInput>();

            if (packagesInside > 0)
                input.Packages.Add(new PackageCostInput() { Id = "PKG1", Weight = 5, Distance = 5, OfferCode = "OFR001" });
            if (packagesInside > 1)
                input.Packages.Add(new PackageCostInput() { Id = "PKG2", Weight = 15, Distance = 5, OfferCode = "OFR002" });
            if (packagesInside > 2)
                input.Packages.Add(new PackageCostInput() { Id = "PKG3", Weight = 10, Distance = 100, OfferCode = "OFR003" });

            if (packagesInside > 3)
            {
                for (int i = 4; i <= packagesInside; i++)
                {
                    input.Packages.Add(new PackageCostInput() { Id = $"PKG{i}", Weight = i * 5, Distance = i * 15, OfferCode = $"OFR00{i}" });
                }
            }

            return input;
        }

        private static IEnumerable<Offer> GetSampleOffers()
        {
            return new List<Offer>() {
                new Offer
                {
                    DiscountPercentage = 10,
                    Name = "OFR001",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 200, Minimum = 0 },
                        Weight = new NumericalRange<int>() { Maximum = 200, Minimum = 70 }
                    }
                },
                new Offer
                {
                    DiscountPercentage = 7,
                    Name = "OFR002",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 150, Minimum = 50 },
                        Weight = new NumericalRange<int>() { Maximum = 250, Minimum = 100 }
                    }
                },
                new Offer
                {
                    DiscountPercentage = 5,
                    Name = "OFR003",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 250, Minimum = 50 },
                        Weight = new NumericalRange<int>() { Maximum = 150, Minimum = 10 }
                    }
                },
            };
        }

        private static IEnumerable<Offer> GetSampleOffers2()
        {
            return new List<Offer>() {
                new Offer
                {
                    DiscountPercentage = 10,
                    Name = "O1",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 200, Minimum = 0 },
                        Weight = new NumericalRange<int>() { Maximum = 200, Minimum = 70 }
                    }
                },
                new Offer
                {
                    DiscountPercentage = 7,
                    Name = "O2",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 150, Minimum = 50 },
                        Weight = new NumericalRange<int>() { Maximum = 250, Minimum = 100 }
                    }
                },
                new Offer
                {
                    DiscountPercentage = 5,
                    Name = "O3",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 250, Minimum = 50 },
                        Weight = new NumericalRange<int>() { Maximum = 150, Minimum = 10 }
                    }
                },
                new Offer
                {
                    DiscountPercentage = 5,
                    Name = "O4",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 250, Minimum = 50 },
                        Weight = new NumericalRange<int>() { Maximum = 150, Minimum = 10 }
                    }
                },
                new Offer
                {
                    DiscountPercentage = 5,
                    Name = "O5",
                    Criteria = new OfferCriteria()
                    {
                        Distance = new NumericalRange<int>() { Maximum = 250, Minimum = 50 },
                        Weight = new NumericalRange<int>() { Maximum = 150, Minimum = 10 }
                    }
                }
            };
        }

        public CostEstimateOutput GetSampleOutput(CostEstimateInput input)
        {
            CostEstimateOutput expected = new CostEstimateOutput();
            expected.PackageOutputs = new List<PackageCostOutput>();
            foreach (var pkg in input.Packages)
            {
                expected.PackageOutputs.Add(new PackageCostOutput()
                {
                    Cost = 0,
                    Discount = 0,
                    Id = pkg.Id
                });
            }

            return expected;
        }
    }
}

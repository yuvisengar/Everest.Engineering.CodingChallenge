namespace Everest.Engineering.Business.Models
{
    public class PackageTimeOutput : PackageCostOutput
    {
        public double DeliveryTime { get; set; }

        public PackageTimeOutput() { }

        public PackageTimeOutput(PackageCostOutput packageCostOutput)
        {
            this.Cost = packageCostOutput.Cost;
            this.Discount = packageCostOutput.Discount;
            this.Id = packageCostOutput.Id;
        }

        public override string ToString()
        {
            return $"{Id}-{Discount}%-Rs.{Cost}-{DeliveryTime}hr";
        }
    }
}

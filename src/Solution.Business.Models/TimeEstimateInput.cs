namespace Everest.Engineering.Business.Models
{
    public class TimeEstimateInput : CostEstimateInput
    {
        public VehiclesInput Vehicles { get; set; }

        public override string ToString()
        {
            var baseString = $"TotalPackages-{Packages.Count}; Cost-{BaseDeliveryCost};";
            return $"{baseString} Vehicles-{Vehicles}";
        }
    }
}


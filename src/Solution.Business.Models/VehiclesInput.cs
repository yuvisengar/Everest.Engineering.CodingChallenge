namespace Everest.Engineering.Business.Models
{
    public class VehiclesInput
    {
        public int TotalCount { get; set; }

        public int MaxSpeed { get; set; }

        public int MaxWeightCapacity { get; set; }

        public override string ToString()
        {
            return $"Count-{TotalCount}; Speed-{MaxSpeed}kmph; Weight-{MaxWeightCapacity}kg";
        }
    }
}

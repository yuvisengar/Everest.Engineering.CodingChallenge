namespace Everest.Engineering.Business.Models
{
    public class Vehicle
    {
        public bool IsFree { get { return TimeToBeFree == 0; } }

        public double TimeToBeFree { get; set; }

        public int Id { get; set; }

        public double TimeTraveled { get; set; }

        public override string ToString()
        {
            return $"No-{Id},FreeIn-{TimeToBeFree}hr,Traveled-{TimeTraveled}hr";
        }
    }
}

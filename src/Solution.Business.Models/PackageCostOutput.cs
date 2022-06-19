namespace Everest.Engineering.Business.Models
{
    public class PackageCostOutput
    {
        public string Id { get; set; }
        public int Discount { get; set; }
        public int Cost { get; set; }

        public override string ToString()
        {
            return $"{Id}-{Discount}%-Rs.{Cost}";
        }
    }
}

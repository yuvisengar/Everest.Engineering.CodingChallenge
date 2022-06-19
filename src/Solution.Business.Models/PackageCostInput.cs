namespace Everest.Engineering.Business.Models
{
    public class PackageCostInput
    {
        public string Id { get; set; }
        public int Weight { get; set; }
        public int Distance { get; set; }
        public string OfferCode { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsAllreadySelected { get; set; }

        public override string ToString()
        {
            return $"{Id}-{Weight}kg-{Distance}km-{OfferCode}";
        }
    }
}

namespace Everest.Engineering.Data.Models
{
    public class Offer: DbEntity
    {  
        public string Name { get; set; }

        public int DiscountPercentage { get; set; }

        public OfferCriteria  Criteria { get; set; }

        public override string ToString()
        {
            return $"{Name}({DiscountPercentage}%) - {Criteria}";
        }
    }
}

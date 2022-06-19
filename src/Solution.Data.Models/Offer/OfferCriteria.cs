namespace Everest.Engineering.Data.Models
{
    public class OfferCriteria
    {
        public NumericalRange<int> Distance { get; set; }

        public NumericalRange<int> Weight { get; set; }

        public override string ToString()
        {
            return $"Distance-{Distance} Weight-{Weight}";
        }
    }
}

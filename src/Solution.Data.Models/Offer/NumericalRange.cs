namespace Everest.Engineering.Data.Models
{
    public class NumericalRange<T>
    {
        public T Minimum { get; set; }

        public T Maximum { get; set; }

        public override string ToString()
        {
            return $"Min-{Minimum} Max-{Maximum}";
        }
    }
}

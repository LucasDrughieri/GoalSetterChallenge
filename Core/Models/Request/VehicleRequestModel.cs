
namespace Core.Models.Request
{
    public class VehicleRequestModel
    {
        public string Brand { get; set; }

        public int Year { get; set; }

        public double? PricePerDay { get; set; }

        public override string ToString()
        {
            return $"Brand: {Brand}, Year: {Year}, PricePerDay: {PricePerDay}";
        }
    }
}

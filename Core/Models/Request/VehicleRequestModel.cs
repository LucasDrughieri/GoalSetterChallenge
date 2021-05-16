
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Request
{
    public class VehicleRequestModel
    {
        [Required]
        public string Brand { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public double? PricePerDay { get; set; }

        public override string ToString()
        {
            return $"Brand: {Brand}, Year: {Year}, PricePerDay: {PricePerDay}";
        }
    }
}

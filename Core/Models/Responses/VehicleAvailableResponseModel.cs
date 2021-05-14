namespace Core.Models.Responses
{
    public class VehicleAvailableResponseModel
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public int Year { get; set; }

        public double PricePerDay { get; set; }
    }
}

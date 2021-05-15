using System;

namespace Core.Models.Request
{
    public class CreateRentalRequestModel
    {
        public int? ClientId { get; set; }

        public int? VehicleId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public override string ToString()
        {
            return $"ClientId: {ClientId}, VehicleId: {VehicleId}, StartDate: {StartDate}, EndDate: {EndDate}";
        }
    }
}

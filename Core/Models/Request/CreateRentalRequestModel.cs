using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Request
{
    public class CreateRentalRequestModel
    {
        [Required]
        public int? ClientId { get; set; }

        [Required]
        public int? VehicleId { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        public override string ToString()
        {
            return $"ClientId: {ClientId}, VehicleId: {VehicleId}, StartDate: {StartDate}, EndDate: {EndDate}";
        }
    }
}

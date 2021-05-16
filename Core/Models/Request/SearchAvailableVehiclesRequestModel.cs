using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Request
{
    public class SearchAvailableVehiclesRequestModel
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        public override string ToString()
        {
            return $"StartDate: {StartDate}, EndDate: {EndDate}";
        }
    }
}

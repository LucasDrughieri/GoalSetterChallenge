using System;

namespace Core.Models.Request
{
    public class SearchAvailableVehiclesRequestModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override string ToString()
        {
            return $"StartDate: {StartDate}, EndDate: {EndDate}";
        }
    }
}

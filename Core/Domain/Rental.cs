using Core.Enums;
using System;

namespace Core.Domain
{
    public class Rental : BaseEntity
    {
        public int ClientId { get; set; }

        public Client Client { get; set; }

        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Price { get; set; }

        public RentalStatus Status { get; set; }
    }
}

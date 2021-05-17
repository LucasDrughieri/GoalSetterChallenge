using Core.Enums;
using System;

namespace Core.Domain
{
    public class Rental : BaseEntity
    {
        /// <summary>
        /// ClientId who rent the vehicle
        /// </summary>
        public int ClientId { get; set; }

        public Client Client { get; set; }

        /// <summary>
        /// VehicleId of the rented vehicle
        /// </summary>
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        /// <summary>
        /// Start Date of the rental
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date of the rental
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Represents the total price of the rental
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Represents the status of the rental. Values: Reserved, Cancelled and Returned
        /// </summary>
        public RentalStatus Status { get; set; }
    }
}

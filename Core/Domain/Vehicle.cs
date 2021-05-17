using Core.Interfaces;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Vehicle : BaseEntity, ILogicalDelete
    {
        /// <summary>
        /// Represents the brand of the vehicle
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Represents the year of the vehicle
        /// </summary>
        public int Year { get; set; }

        public IList<Rental> Rentals { get; set; }

        /// <summary>
        /// Represents the vehicle is available to be rented
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Represents the price per day of the vehicle
        /// </summary>
        public double PricePerDay { get; set; }
    }
}

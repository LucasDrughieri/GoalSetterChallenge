using Core.Interfaces;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Client : BaseEntity, ILogicalDelete
    {
        /// <summary>
        /// Represents the name of the client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the phone of the client
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// All rentals made for the client
        /// </summary>
        public IList<Rental> Rentals { get; set; }

        /// <summary>
        /// Represents if the client is available to rent a vehicle
        /// </summary>
        public bool Active { get; set; }
    }
}

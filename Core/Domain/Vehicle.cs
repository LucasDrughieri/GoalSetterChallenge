using Core.Interfaces;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Vehicle : BaseEntity, ILogicalDelete
    {
        public string Brand { get; set; }

        public int Year { get; set; }

        public IList<Rental> Rentals { get; set; }

        public bool Active { get; set; }

        public double PricePerDay { get; set; }
    }
}

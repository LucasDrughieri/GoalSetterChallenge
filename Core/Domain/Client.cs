using Core.Interfaces;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Client : BaseEntity, ILogicalDelete
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public IList<Rental> Rentals { get; set; }

        public bool Active { get; set; }
    }
}

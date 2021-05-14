using Core.Domain;
using System;
using System.Collections.Generic;

namespace Core.Interfaces.Repository
{
    public interface IRentalRepository
    {
        IList<Vehicle> GetVehiclesAvailables(DateTime startDate, DateTime endDate);

        void Add(Rental rental);

        void Update(Rental rental);

        Rental Find(int id);
    }
}

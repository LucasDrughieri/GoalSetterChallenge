using Core.Configuration;
using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _appDbContext;

        public RentalRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Rental rental)
        {
            _appDbContext.Rentals.Add(rental);
        }

        public void Update(Rental rental)
        {
            _appDbContext.Rentals.Update(rental);
        }

        public Rental Find(int id)
        {
            return _appDbContext.Rentals.Find(id);
        }

        public IList<Vehicle> GetVehiclesAvailables(DateTime startDate, DateTime endDate)
        {
            var vehicleIdRented = _appDbContext.Rentals
                .Where(x => x.Status == RentalStatus.Reserved && ((startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate)))
                .Select(x => x.VehicleId)
                .ToList();

            return _appDbContext.Vehicles.Where(x => x.Active && !vehicleIdRented.Contains(x.Id)).ToList();
        }

        public bool VerifyIfVehicleIsAvailableByRangeDates(int id, DateTime startDate, DateTime endDate)
        {
            return !_appDbContext.Rentals.Any(x => x.VehicleId == id && x.Status == RentalStatus.Reserved && ((startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate)));
        }
    }
}

using Core.Configuration;
using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    /// <summary>
    /// Repository to access to rental data on database
    /// </summary>
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext appDbContext;

        public RentalRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Add new rental on database
        /// </summary>
        /// <param name="rental"></param>
        public void Add(Rental rental)
        {
            appDbContext.Rentals.Add(rental);
        }

        /// <summary>
        /// Update a rental on database
        /// </summary>
        /// <param name="rental"></param>
        public void Update(Rental rental)
        {
            appDbContext.Rentals.Update(rental);
        }

        /// <summary>
        /// Find a rental by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Rental</returns>
        public Rental Find(int id)
        {
            return appDbContext.Rentals.Find(id);
        }

        /// <summary>
        /// Get available vehicles from database by range dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>IList<Vehicle></returns>
        public IList<Vehicle> GetVehiclesAvailables(DateTime startDate, DateTime endDate)
        {
            var vehicleIdRented = appDbContext.Rentals
                .Where(x => x.Status == RentalStatus.Reserved && ((startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate)))
                .Select(x => x.VehicleId)
                .ToList();

            return appDbContext.Vehicles.Where(x => x.Active && !vehicleIdRented.Contains(x.Id)).ToList();
        }

        /// <summary>
        /// Check if a specific vehicle is available in a range dates
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public bool VerifyIfVehicleIsAvailableByRangeDates(int id, DateTime startDate, DateTime endDate)
        {
            return !appDbContext.Rentals.Any(x => x.VehicleId == id && x.Status == RentalStatus.Reserved && ((startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate)));
        }
    }
}

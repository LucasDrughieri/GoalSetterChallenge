using Core.Configuration;
using Core.Domain;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Repository
{
    /// <summary>
    /// Repository to access to vehicle data on database
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext appDbContext;

        public VehicleRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Add new vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public void Add(Vehicle vehicle)
        {
            appDbContext.Vehicles.Add(vehicle);
        }

        /// <summary>
        /// Delete vehicle from database
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Vehicle entity)
        {
            if (entity is not ILogicalDelete)
                appDbContext.Vehicles.Remove(entity);
            else
            {
                entity.Active = false;
                appDbContext.Vehicles.Update(entity);
            }
        }

        /// <summary>
        /// Find vehicle by id
        /// </summary>
        /// <param name="id"></param>
        public Vehicle Find(int id)
        {
            return appDbContext.Vehicles.Find(id);
        }
    }
}

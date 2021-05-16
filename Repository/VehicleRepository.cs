using Core.Configuration;
using Core.Domain;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext appDbContext;

        public VehicleRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void Add(Vehicle vehicle)
        {
            appDbContext.Vehicles.Add(vehicle);
        }

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

        public Vehicle Find(int id)
        {
            return appDbContext.Vehicles.Find(id);
        }
    }
}

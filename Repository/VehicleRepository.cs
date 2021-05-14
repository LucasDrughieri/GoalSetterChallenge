using Core.Configuration;
using Core.Domain;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _appDbContext;

        public VehicleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Vehicle vehicle)
        {
            _appDbContext.Vehicles.Add(vehicle);
        }

        public void Delete(Vehicle entity)
        {
            if (entity is not ILogicalDelete)
                _appDbContext.Vehicles.Remove(entity);
            else
            {
                entity.Active = false;
                _appDbContext.Vehicles.Update(entity);
            }
        }

        public Vehicle Find(int id)
        {
            return _appDbContext.Vehicles.Find(id);
        }
    }
}

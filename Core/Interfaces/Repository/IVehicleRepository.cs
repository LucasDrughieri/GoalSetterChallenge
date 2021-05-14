using Core.Domain;

namespace Core.Interfaces.Repository
{
    public interface IVehicleRepository
    {
        void Add(Vehicle vehicle);
        Vehicle Find(int id);
        void Delete(Vehicle entity);
    }
}

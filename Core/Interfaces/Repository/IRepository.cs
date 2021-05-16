namespace Core.Interfaces.Repository
{
    public interface IRepository
    {
        IClientRepository ClientRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IRentalRepository RentalRepository { get; }
        void Save();
    }
}

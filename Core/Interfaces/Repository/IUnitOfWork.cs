namespace Core.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        IClientRepository ClientRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IRentalRepository RentalRepository { get; }
        void Save();
    }
}

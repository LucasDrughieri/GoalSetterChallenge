using Core.Configuration;
using Core.Interfaces.Repository;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;
        private readonly IClientRepository clientRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IRentalRepository rentalRepository;

        public UnitOfWork(AppDbContext context, IClientRepository clientRepository, IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
        {
            appDbContext = context;
            this.clientRepository = clientRepository;
            this.vehicleRepository = vehicleRepository;
            this.rentalRepository = rentalRepository;
        }

        public IClientRepository ClientRepository => clientRepository;

        public IVehicleRepository VehicleRepository => vehicleRepository;

        public IRentalRepository RentalRepository => rentalRepository;

        public void Save()
        {
            appDbContext.SaveChanges();
        }
    }
}

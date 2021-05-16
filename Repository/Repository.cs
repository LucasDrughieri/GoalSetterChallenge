using Core.Configuration;
using Core.Interfaces.Repository;

namespace Repository
{
    public class Repository : IRepository
    {
        private readonly AppDbContext appDbContext;
        private IClientRepository clientRepository;
        private IVehicleRepository vehicleRepository;
        private IRentalRepository rentalRepository;

        public Repository(AppDbContext context, IClientRepository clientRepository, IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
        {
            appDbContext = context;
            this.clientRepository = clientRepository;
            this.vehicleRepository = vehicleRepository;
            this.rentalRepository = rentalRepository;
        }
        public IClientRepository ClientRepository => clientRepository ?? (clientRepository = new ClientRepository(appDbContext));

        public IVehicleRepository VehicleRepository => vehicleRepository ?? (vehicleRepository = new VehicleRepository(appDbContext));

        public IRentalRepository RentalRepository => rentalRepository ?? (rentalRepository = new RentalRepository(appDbContext));

        public void Save()
        {
            appDbContext.SaveChanges();
        }
    }
}

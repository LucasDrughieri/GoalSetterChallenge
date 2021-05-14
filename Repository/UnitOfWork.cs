using Core.Configuration;
using Core.Interfaces.Repository;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IClientRepository _clientRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRepository _rentalRepository;

        public UnitOfWork(AppDbContext context, IClientRepository clientRepository, IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
        {
            _appDbContext = context;
            _clientRepository = clientRepository;
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
        }

        public IClientRepository ClientRepository => _clientRepository;

        public IVehicleRepository VehicleRepository => _vehicleRepository;

        public IRentalRepository RentalRepository => _rentalRepository;

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}

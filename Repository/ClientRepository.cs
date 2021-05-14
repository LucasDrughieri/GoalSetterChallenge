using Core.Configuration;
using Core.Domain;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _appDbContext;

        public ClientRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Client client)
        {
            _appDbContext.Clients.Add(client);
        }

        public void Delete(Client entity)
        {
            if (entity is not ILogicalDelete)
                _appDbContext.Clients.Remove(entity);
            else
            {
                entity.Active = false;
                _appDbContext.Clients.Update(entity);
            }
        }

        public Client Find(int id)
        {
            return _appDbContext.Clients.Find(id);
        }
    }
}

using Core.Configuration;
using Core.Domain;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext appDbContext;

        public ClientRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void Add(Client client)
        {
            appDbContext.Clients.Add(client);
        }

        public void Delete(Client entity)
        {
            if (entity is not ILogicalDelete)
                appDbContext.Clients.Remove(entity);
            else
            {
                entity.Active = false;
                appDbContext.Clients.Update(entity);
            }
        }

        public Client Find(int id)
        {
            return appDbContext.Clients.Find(id);
        }
    }
}

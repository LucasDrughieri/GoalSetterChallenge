using Core.Configuration;
using Core.Domain;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Repository
{
    /// <summary>
    /// Repository to access to client data on database
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext appDbContext;

        public ClientRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Add new client on database
        /// </summary>
        /// <param name="client"></param>
        public void Add(Client client)
        {
            appDbContext.Clients.Add(client);
        }

        /// <summary>
        /// Delete client from database
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        /// Find a client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Client</returns>
        public Client Find(int id)
        {
            return appDbContext.Clients.Find(id);
        }
    }
}

using Core.Domain;

namespace Core.Interfaces.Repository
{
    public interface IClientRepository
    {
        void Add(Client client);
        void Delete(Client entity);
        Client Find(int id);
    }
}

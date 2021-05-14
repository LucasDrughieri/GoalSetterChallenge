using Core.Models;
using Core.Models.Request;

namespace Core.Interfaces.Services
{
    public interface IClientService
    {
        Response Add(ClientRequestModel request);
        Response Delete(int id);
    }
}

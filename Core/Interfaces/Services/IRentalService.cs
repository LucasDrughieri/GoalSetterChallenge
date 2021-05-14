using Core.Models;
using Core.Models.Request;

namespace Core.Interfaces.Services
{
    public interface IRentalService
    {
        Response Add(CreateRentalRequestModel request);
        Response Cancel(int id);
    }
}

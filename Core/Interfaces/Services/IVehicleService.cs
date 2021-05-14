using Core.Models;
using Core.Models.Request;
using Core.Models.Responses;
using System.Collections.Generic;

namespace Core.Interfaces.Services
{
    public interface IVehicleService
    {
        Response Add(VehicleRequestModel request);

        Response Delete(int id);

        Response<IList<VehicleAvailableResponseModel>> GetAvailables(SearchAvailableVehiclesRequestModel request);
    }
}

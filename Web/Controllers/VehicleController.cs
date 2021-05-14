using Core.Interfaces.Services;
using Core.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        [ValidateBodyActionFilter]
        public IActionResult Post([FromBody]VehicleRequestModel request)
        {
            var response = _vehicleService.Add(request);

            return response.CreateResponse(this);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _vehicleService.Delete(id);

            return response.CreateResponse(this);
        }

        [HttpGet("availables")]
        public IActionResult Availables([FromQuery] SearchAvailableVehiclesRequestModel request)
        {
            var response = _vehicleService.GetAvailables(request);

            return response.CreateResponse(this);
        }
    }
}

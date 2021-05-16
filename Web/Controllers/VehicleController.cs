using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateBodyActionFilter]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Post([FromBody] VehicleRequestModel request)
        {
            _logger.LogInformation($"POST /api/vehicle reach with body: {request}");

            var response = _vehicleService.Add(request);

            return response.CreateResponse(this);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE /api/vehicle/{id} reach");

            var response = _vehicleService.Delete(id);

            return response.CreateResponse(this);
        }

        [HttpGet("availables")]
        [ProducesResponseType(typeof(Response<IList<VehicleAvailableResponseModel>>), 200)]
        public IActionResult Availables([FromQuery] SearchAvailableVehiclesRequestModel request)
        {
            _logger.LogInformation($"GET /api/vehicle reach with query params: {request}");

            var response = _vehicleService.GetAvailables(request);

            return response.CreateResponse(this);
        }
    }
}

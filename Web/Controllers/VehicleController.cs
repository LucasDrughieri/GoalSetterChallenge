using System.Collections.Generic;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService vehicleService;
        private readonly ILogger<VehicleController> logger;

        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
        {
            this.vehicleService = vehicleService;
            this.logger = logger;
        }

        /// <summary>
        /// Endpoint to create a new vehicle
        /// </summary>
        [HttpPost]
        [ValidateBodyActionFilter]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Post([FromBody] VehicleRequestModel request)
        {
            logger.LogInformation($"POST /api/vehicle reach with body: {request}");

            var response = vehicleService.Add(request);

            return response.CreateResponse(this);
        }

        /// <summary>
        /// Endpoint to delete a vehicle by id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Delete(int id)
        {
            logger.LogInformation($"DELETE /api/vehicle/{id} reach");

            var response = vehicleService.Delete(id);

            return response.CreateResponse(this);
        }

        /// <summary>
        /// Endpoint to get the vehicle availables by range dates
        /// </summary>
        [HttpGet("availables")]
        [ProducesResponseType(typeof(Response<IList<VehicleAvailableResponseModel>>), 200)]
        public IActionResult Availables([FromQuery] SearchAvailableVehiclesRequestModel request)
        {
            logger.LogInformation($"GET /api/vehicle reach with query params: {request}");

            var response = vehicleService.GetAvailables(request);

            return response.CreateResponse(this);
        }
    }
}

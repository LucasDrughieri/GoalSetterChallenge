using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService rentalService;
        private readonly ILogger<RentalController> logger;

        public RentalController(IRentalService rentalService, ILogger<RentalController> logger)
        {
            this.rentalService = rentalService;
            this.logger = logger;
        }

        /// <summary>
        /// Endpoint to create a new rental
        /// </summary>
        [HttpPost]
        [ValidateBodyActionFilter]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Post([FromBody] CreateRentalRequestModel request)
        {
            logger.LogInformation($"POST /api/rental reach with body: {request}");

            var response = rentalService.Add(request);

            return response.CreateResponse(this);
        }

        /// <summary>
        /// Endpoint to cancel a rental by id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Cancel(int id)
        {
            logger.LogInformation($"Delete /api/rental/{id} reach");

            var response = rentalService.Cancel(id);

            return response.CreateResponse(this);
        }
    }
}

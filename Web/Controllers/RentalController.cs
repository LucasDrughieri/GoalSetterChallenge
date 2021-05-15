using Core.Interfaces.Services;
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
        private readonly IRentalService _rentalService;
        private readonly ILogger<RentalController> _logger;

        public RentalController(IRentalService rentalService, ILogger<RentalController> logger)
        {
            _rentalService = rentalService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateBodyActionFilter]
        public IActionResult Post([FromBody] CreateRentalRequestModel request)
        {
            _logger.LogInformation($"POST /api/rental reach with body: {request}");

            var response = _rentalService.Add(request);

            return response.CreateResponse(this);
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            _logger.LogInformation($"Delete /api/rental/{id} reach");

            var response = _rentalService.Cancel(id);

            return response.CreateResponse(this);
        }
    }
}

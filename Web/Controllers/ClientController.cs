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
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateBodyActionFilter]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Post([FromBody]ClientRequestModel request)
        {
            _logger.LogInformation($"POST /api/client reach with body: {request}");

            var response = _clientService.Add(request);

            return response.CreateResponse(this);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE /api/client/{id} reach");

            var response = _clientService.Delete(id);

            return response.CreateResponse(this);
        }
    }
}

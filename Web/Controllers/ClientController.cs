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
        private readonly IClientService clientService;
        private readonly ILogger<ClientController> logger;

        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            this.clientService = clientService;
            this.logger = logger;
        }

        /// <summary>
        /// Endpoint to create a new client
        /// </summary>
        [HttpPost]
        [ValidateBodyActionFilter]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Post([FromBody]ClientRequestModel request)
        {
            logger.LogInformation($"POST /api/client reach with body: {request}");

            var response = clientService.Add(request);

            return response.CreateResponse(this);
        }

        /// <summary>
        /// Endpoint to delete a client by id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public IActionResult Delete(int id)
        {
            logger.LogInformation($"DELETE /api/client/{id} reach");

            var response = clientService.Delete(id);

            return response.CreateResponse(this);
        }
    }
}

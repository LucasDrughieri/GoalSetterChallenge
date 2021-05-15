using Core.Interfaces.Services;
using Core.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        [ValidateBodyActionFilter]
        public IActionResult Post([FromBody]ClientRequestModel request)
        {
            var response = _clientService.Add(request);

            return response.CreateResponse(this);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _clientService.Delete(id);

            return response.CreateResponse(this);
        }
    }
}

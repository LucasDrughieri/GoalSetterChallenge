using Core.Interfaces.Services;
using Core.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class RentalController : Controller
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        [ValidateBodyActionFilter]
        public IActionResult Post([FromBody] CreateRentalRequestModel request)
        {
            var response = _rentalService.Add(request);

            return response.CreateResponse(this);
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var response = _rentalService.Cancel(id);

            return response.CreateResponse(this);
        }
    }
}

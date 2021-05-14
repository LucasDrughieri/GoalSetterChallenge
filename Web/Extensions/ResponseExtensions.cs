using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions
{
    public static class ResponseExtensions
    {
        public static IActionResult CreateResponse(this Response response, Controller controller)
        {
            if (response.HasErrors()) return controller.BadRequest(response);

            return controller.Ok(response);
        }
    }
}

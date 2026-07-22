using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View("404");
        }

        [Route("500")]
        public IActionResult ServerError()
        {
            return View("500");
        }
    }
}

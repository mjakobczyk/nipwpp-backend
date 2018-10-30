using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Posts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult Index()
        {
            return StatusCode((int)(HttpStatusCode.InternalServerError), new { Error = "Unhandled exception" });
        }
    }
}

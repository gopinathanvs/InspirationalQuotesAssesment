using Microsoft.AspNetCore.Mvc;

namespace InspirationalQuotes.API.Controllers
{

    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello, welcome to Quote Project");
        }
    }
}

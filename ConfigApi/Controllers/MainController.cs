using Microsoft.AspNetCore.Mvc;

namespace ConfigApi.Controllers
{

    public class MainController : Controller
    {

        [HttpGet("/")]
        public IActionResult Get()
        {
            return NoContent();
        }

    }

}

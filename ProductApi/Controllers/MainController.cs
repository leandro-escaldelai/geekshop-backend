using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
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

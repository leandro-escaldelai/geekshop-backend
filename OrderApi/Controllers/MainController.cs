using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
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

using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartApi.Controllers
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

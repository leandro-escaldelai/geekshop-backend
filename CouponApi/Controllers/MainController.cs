using Microsoft.AspNetCore.Mvc;

namespace CouponApi.Controllers
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

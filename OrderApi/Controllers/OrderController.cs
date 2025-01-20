using Microsoft.AspNetCore.Mvc;
using OrderApi.Services;

namespace OrderApi.Controllers
{

    [Route("[controller]")]
    public class OrderController(
        IOrderService _service) : Controller
    {

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            await _service.Create();

            return NoContent();
        }

    }

}

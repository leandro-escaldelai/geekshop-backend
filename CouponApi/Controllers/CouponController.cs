using Microsoft.AspNetCore.Mvc;
using CouponApi.Services;
using CouponApi.Model;

namespace CouponApi.Controllers
{

    [Route("[controller]")]
    public class CouponController(
        ICouponService _service) : Controller
    {

        [HttpGet("{code}"), CustomerAuthorize]
        public async Task<IActionResult> GetByCode(string code)
        {
            var data = await _service.GetByCode(code);

            return Ok(data);
        }

    }
}

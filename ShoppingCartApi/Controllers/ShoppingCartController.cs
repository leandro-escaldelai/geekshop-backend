using ShoppingCartApi.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Services;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.Controllers
{
    
    [Route("shopping-cart")]
    public class ShoppingCartController(
        IShoppingCartService _service) : Controller
    {

        [HttpGet, CustomerAuthorize]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetUserCart();

            return Ok(result);
        }

        [HttpGet("count"), CustomerAuthorize]
        public async Task<IActionResult> CountCartDetails()
        {
            var result = await _service.CountCartDetails();

            return Ok(result);
        }

        [HttpPut("coupon"), CustomerAuthorize]
        public async Task<IActionResult> ApplyCoupon([FromBody] CouponVO coupon)
        {
            var result = await _service.ApplyCoupon(coupon.Code);

            return Ok(result);
        }

        [HttpDelete("coupon"), CustomerAuthorize]
        public async Task<IActionResult> RemoveCoupon()
        {
            await _service.RemoveCoupon();

            return NoContent();
        }

        [HttpPut("item"), CustomerAuthorize]
        public async Task<IActionResult> AddProductToCart([FromBody] CartDetailVO cartDetail)
        {
            if (!cartDetail.ProductId.HasValue)
                return BadRequest("Invalid product Id");

            var result = await _service.AddToCart(cartDetail.ProductId ?? 0);

            return Ok(result);
        }

        [HttpDelete("item/{id}"), CustomerAuthorize]
        public async Task<IActionResult> RemoveProductFromCart(int id)
        {
            await _service.RemoveFromCart(id);

            return NoContent();
        }

        [HttpDelete("item/all"), CustomerAuthorize]
        public async Task<IActionResult> ClearCart()
        {
            await _service.ClearCart();

            return NoContent();
        }

    }

}

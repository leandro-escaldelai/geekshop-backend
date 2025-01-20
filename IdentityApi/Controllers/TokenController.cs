using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityApi.Services;

namespace IdentityApi.Controllers
{

    [Route("[controller]")]
    public class TokenController(
        ITokenService _service) : Controller
    {

        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            var result = _service.Validate(Request);

            return result.IsValid
                ? Ok(result)
                : BadRequest(result);
        }

    }

}

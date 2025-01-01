using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityApi.ValueObject;
using IdentityApi.Services;
using System.Net;
using AutoMapper;

namespace IdentityApi.Controllers
{

    [Route("[controller]")]
    public class LoginController(
        IMapper _mapper,
        ILoginService _loginService) : Controller
    {

        [HttpPost]
        [ProducesResponseType(typeof(TokenVO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostLogin(LoginVO login)
        {
            var result = await _loginService.Login(login);
            var mResult = _mapper.Map<TokenVO>(result);

            return !string.IsNullOrEmpty(mResult.UnauthorizedReason)
                ? Unauthorized(mResult)
                : Ok(mResult);
        }

        [HttpGet("validate"), Authorize]
        public IActionResult ValidateLogin()
        {
            return NoContent();
        }

    }

}

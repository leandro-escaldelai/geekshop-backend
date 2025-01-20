using Microsoft.AspNetCore.Mvc;
using ConfigApi.Services;

namespace ConfigApi.Controllers
{

	[Route("[controller]")]
	public class ConfigController(
		IConfigService service) : Controller
	{

		[HttpGet]
		public async Task<IActionResult> GetList()
		{
			var result = await service.GetList();

			return Ok(result);
		}

	}

}

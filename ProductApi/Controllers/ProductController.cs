using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.ValueObjects;
using ProductApi.Services;
using ProductApi.Model;
using LoggerModels;

namespace ProductApi.Controllers
{


    [Route("[controller]")]
    public class ProductController(
        IAppLogger _logger,
        IProductService _service) : Controller
    {


        [HttpGet]
        [Authorize(Policies.Scope.Get)]
        [Authorize(Policies.Role.Customer)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAll();

            return Ok(products);
        }

        [HttpGet("{id}")]
		[Authorize(Policies.Scope.Get)]
		[Authorize(Policies.Role.Customer)]
		public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("list")]
		[Authorize(Policies.Scope.Get)]
		[Authorize(Policies.Role.Customer)]
		public async Task<IActionResult> GetByIdList([FromQuery] IEnumerable<int> idList)
        {
            var products = await _service.GetByIdList(idList);

            return Ok(products);
        }

        [HttpPost]
		[Authorize(Policies.Scope.Create)]
		[Authorize(Policies.Role.Admin)]
		public async Task<IActionResult> Create([FromBody] ProductVO product)
        {
            if (product == null)
                return BadRequest();

            var createdProduct = await _service.Create(product);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut]
		[Authorize(Policies.Scope.Update)]
		[Authorize(Policies.Role.Admin)]
		public async Task<IActionResult> Update([FromBody] ProductVO product)
        {
            if (product?.Id == null)
                return BadRequest();

            var updatedProduct = await _service.Update(product);

            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
		[Authorize(Policies.Scope.Delete)]
		[Authorize(Policies.Role.Admin)]
		public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetById(id);

            if (product == null)
                return NotFound();

            await _service.Delete(id);

            return NoContent();
        }

    }

}

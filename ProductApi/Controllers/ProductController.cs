using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.ValueObjects;
using ProductApi.Repository;
using ProductApi.Services;

namespace ProductApi.Controllers
{


    [Route("[controller]")]
    public class ProductController(
        IAppLogger _logger,
        IProductRepository _repo) : Controller
    {

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repo.GetAll();

            return Ok(products);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repo.GetById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] ProductVO product)
        {
            if (product == null)
                return BadRequest();

            var createdProduct = await _repo.Create(product);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> Update([FromBody] ProductVO product)
        {
            if (product?.Id == null)
                return BadRequest();

            var updatedProduct = await _repo.Update(product);

            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repo.GetById(id);

            if (product == null)
                return NotFound();

            await _repo.Delete(id);

            return NoContent();
        }

    }

}

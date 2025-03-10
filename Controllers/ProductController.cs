using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Models.response;
using DHP_Manager.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DHP_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return BadRequest(new { message = "No product to list" });
            }

            var productResponses = products.Select(p => new ProductResponse
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category
            }).ToList();

            return Ok(productResponses);
        }

        // GET: api/Product/{name}
        [HttpGet("{name}")]
        public async Task<ActionResult<ProductResponse>> GetProductByName(string name)
        {
            var product = await _productService.GetProductByNameAsync(name);
            if (product == null)
            {
                return BadRequest(new { message = "Product not found" });
            }

            var productResponse = new ProductResponse
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category
            };

            return Ok(productResponse);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest(new { message = "Invalid product data" });
            }

            await _productService.AddProductAsync(ProductDto.ToInput(productDto));
            return Ok();
        }

        // PUT: api/Product/{name}
        [HttpPut("{name}")]
        public async Task<ActionResult> UpdateProduct(string name, [FromBody] ProductDto productDto)
        {
            if (name != productDto.Name)
            {
                return BadRequest(new { message = "Product name mismatch" });
            }

            var existingProduct = await _productService.GetProductByNameAsync(name);
            if (existingProduct == null)
            {
                return BadRequest(new { message = "Product not found" });
            }

            await _productService.UpdateProductAsync(existingProduct.Id, ProductDto.ToInput(productDto));

            return Ok();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductResponse>> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return BadRequest(new { message = "Product not found" });
            }

            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }
}

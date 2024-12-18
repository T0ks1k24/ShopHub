using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Domain;
using ProductManagementApi.Domain.Entity;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _context.Products.AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetByProduct(int id)
        {
            var result = await _context.Products.FindAsync(id);

            if (result == null)
            {
                return NotFound("Product not found");
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is invalid");
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetByProduct), new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest("ID mismatch");
            }
            var existingProduct = await _context.Products.FindAsync(id);

            if(existingProduct == null)
            {
                return BadRequest("Product not found");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return Ok("Product has been updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product has been deleted");
        }

        [HttpGet]
        [Route("by-category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
            {
                return NotFound("Category not found!");
            }

            var products = await _context.Products
                .Where(p => p.CategoryId == category.Id)
                .ToListAsync();

            if(products == null)
            {
                return NotFound("Products not found!");
            }

            return Ok(products);
        }
    }
}

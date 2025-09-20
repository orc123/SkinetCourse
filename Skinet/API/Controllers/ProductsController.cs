using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort) 
        => Ok(await productRepository.GetProductsAsync(brand, type, sort));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
             return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
    {
        productRepository.AddProduct(product);

        if (await productRepository.SaveChangesAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody]Product product)
    {
        if (product.Id != id || !ProductExists(id)) 
            return BadRequest("Cannot update this product");

        productRepository.UpdateProduct(product);

        if (await productRepository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null) 
            return NotFound();
        
        productRepository.DeleteProduct(product);
        if (await productRepository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands() 
        => Ok(await productRepository.GetBrandsAsync());

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes() 
        => Ok(await productRepository.GetTypesAsync());

    private bool ProductExists(int id) => productRepository.ProductExists(id);
}

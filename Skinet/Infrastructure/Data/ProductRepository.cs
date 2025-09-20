using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync() => 
        await context.Products.Select(p => p.Brand)
            .Distinct()
            .ToListAsync();

    public async Task<Product?> GetProductByIdAsync(int id) =>
                await context.Products.FindAsync(id);

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p => p.Brand == brand);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(p => p.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = sort.ToLower() switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
        }
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync() =>
          await context.Products.Select(p => p.Type)
            .Distinct()
            .ToListAsync();

    public bool ProductExists(int id) =>
                context.Products.Any(p => p.Id == id);

    public async Task<bool> SaveChangesAsync() =>
        await context.SaveChangesAsync() > 0;

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}

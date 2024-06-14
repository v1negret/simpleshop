using Microsoft.EntityFrameworkCore;
using SimpleShop.Cache.Interfaces;
using SimpleShop.Data;
using SimpleShop.Models;
using SimpleShop.Models.Dto;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    private readonly IDistributedCacheService _cache;

    public ProductService(AppDbContext db, IDistributedCacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<bool> Add(AddProductDto product)
    {
        var isExists = await _db.Products.FirstOrDefaultAsync(p => p.Name == product.Name);
        if (isExists != null)
            return false;
        var productEntity = new Product()
        {
            Name = product.Name,
            Amount = product.Amount,
            Description = product.Description
        };
        
        await _db.Products.AddAsync(productEntity);
        await _db.SaveChangesAsync();
        return true;

    }

    public async Task<Product> GetById(int id)
    {
        var inCacheProduct = await _cache.GetData<Product>($"product-{id}");
        if (inCacheProduct is not null)
            return inCacheProduct;
        var result = 
            await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        await _cache.SetData($"product-{id}", result, TimeSpan.FromSeconds(30));
        return result;
    }

    public async Task<Product> GetByName(string name)
    {
        var inCacheProduct = await _cache.GetData<Product>($"product-name-{name}");
        if (inCacheProduct is not null)
            return inCacheProduct;
        var result =
            await _db.Products.FirstOrDefaultAsync(p => p.Name == name);
        await _cache.SetData($"product-name-{name}", result, TimeSpan.FromSeconds(30));
        return result;
    }

    public async Task<List<Product>> GetAll()
    {
        var result = await _db.Products.ToListAsync();
        return result;
    }

    public async Task<bool> Remove(Product product)
    {
        var isExist = await _db.Products.ContainsAsync(product);
        if (isExist is false)
            return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Product product)
    {
        var isExist = await _db.Products.ContainsAsync(product);
        if (isExist is false)
            return false;
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        return true;
    }
}
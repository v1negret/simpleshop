using SimpleShop.Models;
using SimpleShop.Models.Dto;

namespace SimpleShop.Services.Interfaces;

public interface IProductService
{
    Task<bool> Add(AddProductDto product);
    Task<Product> GetById(int id);
    Task<Product> GetByName(string name);
    Task<List<Product>> GetAll();
    Task<bool> Remove(Product product);
    Task<bool> Update(Product product);
}
using SimpleShop.Models;
using SimpleShop.Models.Dto;

namespace SimpleShop.Services.Interfaces;

public interface IOrderService
{
    Task<bool> Add(AddOrderDto request);
    Task<Order> GetById(int id);
    Task<Order> GetByUserId(string userId);
    Task<List<Order>> GetAll();
    Task<bool> Remove(Order order);
    Task<bool> Update(Order order);
}
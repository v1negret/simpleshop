using Microsoft.EntityFrameworkCore;
using SimpleShop.Cache.Interfaces;
using SimpleShop.Data;
using SimpleShop.Models;
using SimpleShop.Models.Dto;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;
    private readonly IDistributedCacheService _cache;

    public OrderService(AppDbContext db, IDistributedCacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<bool> Add(AddOrderDto request)
    {
        var isExists =
            await _db.Orders.FirstOrDefaultAsync(o => o.UserId == request.UserId);
        if (isExists is not null)
            return false;
        var order = new Order()
        {
            UserId = request.UserId,
            OrderProducts = request.Products
        };
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Order> GetById(int id)
    {
        var inCacheOrder = await _cache.GetData<Order>($"order-{id}");
        if (inCacheOrder is not null)
            return inCacheOrder;
        var result =
            await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        await _cache.SetData($"order-{id}", result, TimeSpan.FromSeconds(30));
        return result;
    }

    public async Task<Order> GetByUserId(string userId)
    {
        var inCacheOrder = await _cache.GetData<Order>($"order-userid-{userId}");
        if (inCacheOrder is not null)
            return inCacheOrder;
        var result =
            await _db.Orders.FirstOrDefaultAsync(o => o.UserId == userId);
        await _cache.SetData($"order-userid-{userId}", result, TimeSpan.FromSeconds(30));
        return result;
    }

    public async Task<List<Order>> GetAll()
    {
        var result = await _db.Orders.ToListAsync();
        return result;
    }

    public async Task<bool> Remove(Order order)
    {
        var isExist = await _db.Orders.ContainsAsync(order);
        if (isExist is false)
            return false;
        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Order order)
    {
        var isExist = await _db.Orders.ContainsAsync(order);
        if (isExist is false)
            return false;
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
        return true;
    }
}
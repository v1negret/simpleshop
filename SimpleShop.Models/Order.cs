using Microsoft.AspNetCore.Identity;

namespace SimpleShop.Models;

public class Order
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public List<Product>? OrderProducts;
    public IdentityUser User;
}
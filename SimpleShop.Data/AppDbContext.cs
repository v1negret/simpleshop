using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Models;

namespace SimpleShop.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
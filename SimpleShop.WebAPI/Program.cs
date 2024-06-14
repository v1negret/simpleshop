using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SimpleShop.Cache;
using SimpleShop.Cache.Interfaces;
using SimpleShop.Data;
using SimpleShop.Services;
using SimpleShop.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<AppDbContext>(opt 
    => opt.UseNpgsql(config.GetConnectionString("DbConnection")));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = config.GetConnectionString("Redis");
});

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDistributedCacheService, DistributedCacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.Run();
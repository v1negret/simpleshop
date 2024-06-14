using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Models;
using SimpleShop.Models.Dto;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("all/")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _productService.GetAll();
        return Ok(response);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var response = await _productService.GetById(id);
        if (response is null)
            return BadRequest();
        return Ok(response);
    }
    [HttpGet("get/name/{name}")]
    public async Task<IActionResult> GetByName([FromRoute]string name)
    {
        var response = await _productService.GetByName(name);
        if (response is null)
            return BadRequest();
        return Ok(response);
    }

    [HttpPost("add/")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(AddProductDto request)
    {
        var response = await _productService.Add(request);
        if (!response)
            return BadRequest();
        return Ok();
    }

    [HttpDelete("delete/")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Remove([FromRoute]Product request)
    {
        var response = await _productService.Remove(request);
        if (!response)
            return BadRequest();
        return Ok();
    }
    [HttpPatch("update/")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute]Product request)
    {
        var response = await _productService.Update(request);
        if (!response)
            return BadRequest();
        return Ok();
    }
}
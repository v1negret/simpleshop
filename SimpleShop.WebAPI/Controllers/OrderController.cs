using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Models;
using SimpleShop.Models.Dto;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly UserManager<IdentityUser> _userManager;
    public OrderController(IOrderService orderService, UserManager<IdentityUser> userManager)
    {
        _orderService = orderService;
        _userManager = userManager;
    }

    [HttpGet("get/")]
    [Authorize]
    public async Task<IActionResult> GetByUserId()
    {
        var user = await _userManager.GetUserAsync(User);
        var response = await _orderService.GetByUserId(user.Id);
        if (response is null)
            return BadRequest();
        return Ok(response);
    }

    [HttpPost("add/")]
    [Authorize]
    public async Task<IActionResult> Add([FromBody]AddOrderDto request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (request.UserId != user.Id)
            return Forbid();
        var response = await _orderService.Add(request);
        if (!response)
            return BadRequest();
        return Ok();
    }
    
    [HttpPatch("update/")]
    [Authorize]
    public async Task<IActionResult> Update(Order request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (request.UserId != user.Id)
            return Forbid();
        var response = await _orderService.Update(request);
        if (!response)
            return BadRequest();
        return Ok();
    }
    
    [HttpDelete("remove/")]
    [Authorize]
    public async Task<IActionResult> Remove(Order request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (request.UserId != user.Id)
            return Forbid();
        var response = await _orderService.Remove(request);
        if (!response)
            return BadRequest();
        return Ok();
    }
}
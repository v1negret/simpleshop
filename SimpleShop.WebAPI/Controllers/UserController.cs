using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SimpleShop.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserController(IConfiguration config, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _config = config;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("set-admin/{code}")]
    [Authorize]
    public async Task<ActionResult> SetAdminRole([FromRoute]string code)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (code == _config["SecretKey"])
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok();
        }

        return BadRequest();
    }
}
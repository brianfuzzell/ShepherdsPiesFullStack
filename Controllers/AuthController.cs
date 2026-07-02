using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShepherdsPiesControllers.DTOs;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<Employee> _signInManager;
    private readonly UserManager<Employee> _userManager;

    public AuthController(SignInManager<Employee> signInManager, UserManager<Employee> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<EmployeeResponseDto>> Login(LoginDto loginDto)
    {
        var employee = await _userManager.FindByEmailAsync(loginDto.Email);
        if (employee is null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.PasswordSignInAsync(employee, loginDto.Password, isPersistent: true, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        return Ok(await BuildProfileAsync(employee));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpGet("login/profile")]
    [Authorize]
    public async Task<ActionResult<EmployeeResponseDto>> Profile()
    {
        var employee = await _userManager.GetUserAsync(User);
        if (employee is null)
        {
            return Unauthorized();
        }

        return Ok(await BuildProfileAsync(employee));
    }

    private async Task<EmployeeResponseDto> BuildProfileAsync(Employee employee)
    {
        var roles = await _userManager.GetRolesAsync(employee);

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }
}
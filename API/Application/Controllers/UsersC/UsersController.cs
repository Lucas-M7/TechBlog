using API.Domain.DTOs;
using API.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers.UsersC;

[ApiController]
[Route("api/")]
public class UsersController(UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager)
    : ControllerBase
{
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly SignInManager<UserModel> _signInManager = signInManager;

    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new UserModel
        { UserName = registerDTO.Email, Email = registerDTO.Email, Age = registerDTO.Age };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User registered sucessfully.");
    }

    [HttpPost("users/login")]
    public async Task<IActionResult> Login([FromQuery] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _signInManager.PasswordSignInAsync(
            loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Unauthorized("Invalid login attempt.");        

        return Ok("Login sucessfully.");    
    }

    [HttpPost("users/logout")]
    public async Task<IActionResult> Logout([FromBody] object empty)
    {
        await _signInManager.SignOutAsync();
        return Ok("Logout sucessfully.");
    }

};
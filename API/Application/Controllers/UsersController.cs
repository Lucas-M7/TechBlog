using System.Security.Claims;
using API.Domain.DTOs;
using API.Domain.Models.User;
using API.Infrasctuture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[ApiController]
[Route("api/")]
public class UsersController(UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager, AppDbContext context)
    : ControllerBase
{
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly SignInManager<UserModel> _signInManager = signInManager;
    private readonly AppDbContext _context = context;

    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new UserModel
        { UserName = registerDTO.Username, Email = registerDTO.Email, Age = registerDTO.Age };

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
            loginDTO.Username, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Unauthorized("Invalid login attempt.");

        return Ok("Login sucessfully.");
    }

    [Authorize]
    [HttpPost("users/logout")]
    public async Task<IActionResult> Logout([FromBody] object empty)
    {
        await _signInManager.SignOutAsync();
        return Ok("Logout sucessfully.");
    }

    [Authorize]
    [HttpGet("users/posts")]
    public IActionResult ListUserEspecifPost([FromQuery] int? page)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = _context.Posts.Where(p => p.UserId == userId);

        int postsForPage = 20;

        if (page != null && page > 0)
            query = query.Skip(((int)page - 1) * postsForPage).Take(postsForPage);

        var result = query.Select(post => new
        {
            post.PostId,
            post.Title,
            post.Content,
            post.Username
        }).ToList();

        return Ok(result);
    }
};
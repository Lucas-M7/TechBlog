using System.Security.Claims;
using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.User;
using API.Infrasctuture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[ApiController]
[Route("api/")]
public class UsersController(UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager, AppDbContext context,
    IPostService postService, IUserService userService)
    : ControllerBase
{
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly SignInManager<UserModel> _signInManager = signInManager;
    private readonly AppDbContext _context = context;
    private readonly IPostService _postService = postService;
    private readonly IUserService _userService = userService;

    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.Register(registerDTO);

        return Ok("User registered sucessfully.");
    }

    [HttpPost("users/login")]
    public async Task<IActionResult> Login([FromQuery] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _userService.Login(loginDTO);

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
    [HttpPut("users")]
    public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameDTO changeUsernameDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var passwordCheck = await _userManager.CheckPasswordAsync(user, changeUsernameDTO.CurrentPassword);
        if (!passwordCheck)
            return BadRequest("Incorret password.");

        // Changing old username for new username
        user.UserName = changeUsernameDTO.NewUsername;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest("Failed to change username.");

        await _postService.UpdateUsernameInPosts(userId, changeUsernameDTO.NewUsername);
        return Ok("Username changed sucessfully.");
    }

    [Authorize]
    [HttpGet("users/posts")]
    public IActionResult ListUserEspecifPost([FromQuery] int? page)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = _context.Posts.Where(p => p.UserId == userId);

        int postsForPage = 10;

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
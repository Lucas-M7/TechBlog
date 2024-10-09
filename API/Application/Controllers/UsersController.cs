using System.Security.Claims;
using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.User;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[ApiController]
[Route("api/")]
public class UsersController : ControllerBase
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly AppDbContext _context;
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public UsersController(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            AppDbContext context,
            IPostService postService,
            IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _postService = postService;
        _userService = userService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDTO">The registration details.</param>
    /// <returns>Action result indicating the outcome of the registration process.</returns>
    [HttpPost("users")]
    public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.Register(registerDTO);

        return Ok("User registered sucessfully.");
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginDTO">The login details.</param>
    /// <returns>Action result indicating the outcome of the login process.</returns>
    [HttpPost("users/login")]
    public async Task<IActionResult> Login([FromQuery] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.Login(loginDTO);

        return Ok("Login sucessfully.");
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <returns>Action result indicating the outcome of the logout process.</returns>
    [Authorize]
    [HttpPost("users/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logout sucessfully.");
    }

    /// <summary>
    /// Changes the username of the current user.
    /// </summary>
    /// <param name="changeUsernameDTO">The details required to change the username.</param>
    /// <returns>Action result indicating the outcome of the username change process.</returns>
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


        user.UserName = changeUsernameDTO.NewUsername;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest("Failed to change username.");

        await _postService.UpdateUsernameInPosts(userId, changeUsernameDTO.NewUsername);
        return Ok("Username changed sucessfully.");
    }

    /// <summary>
    /// Lists posts of the current user with optional pagination.
    /// </summary>
    /// <param name="page">The page number for pagination (optional).</param>
    /// <returns>A list of posts created by the current user.</returns>
    [Authorize]
    [HttpGet("users/posts")]
    public IActionResult ListUserEspecifPost([FromQuery] int? page)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = _context.Posts.Where(p => p.UserId == userId);

        const int postsPerPage = 10;

        if (page.HasValue && page > 0)
            query = query.Skip((page.Value - 1) * postsPerPage).Take(postsPerPage);

        var result = query.Select(post => new
        {
            post.PostId,
            post.Title,
            post.Content,
            post.Username
        }).ToList();

        return Ok(result);
    }

    /// <summary>
    /// Deletes the current user's account.
    /// </summary>
    /// <param name="deleteUserDTO">The details required to delete the account.</param>
    /// <returns>Action result indicating the outcome of the account deletion process.</returns>
    [Authorize]
    [HttpDelete("users")]
    public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserDTO deleteUserDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var passwordCheck = await _userManager.CheckPasswordAsync(user, deleteUserDTO.CurrentPassword);
        if (!passwordCheck)
            return BadRequest("Incorret password.");

        var result = await _userManager.DeleteAsync(user);
        await _signInManager.SignOutAsync();


        if (!result.Succeeded)
            return BadRequest("Failed to delete your account.");

        return Ok("Account deleted sucessfully!");
    }
};
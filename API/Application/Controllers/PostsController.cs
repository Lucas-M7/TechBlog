using System.Security.Claims;
using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Domain.Models.User;
using API.Domain.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[ApiController]
[Route("api/")]
public class PostsController(IPostService postService, UserManager<UserModel> userManager) : ControllerBase
{
    private readonly IPostService _postService = postService;
    private readonly UserManager<UserModel>  _userManager = userManager;

    [Authorize]
    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost([FromBody] PostDTO postDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var post = new PostModel
        {
            Title = postDTO.Title,
            Content = postDTO.Content,
            UserId = userId,
            Username = user.UserName
        };

        var result = new PostView
        {
            Title = postDTO.Title,
            Content = postDTO.Content,
            Author = user.UserName
        };

        _postService.CreatePost(post);
        return Created($"Post created sucessfuly: ", result);
    }
}
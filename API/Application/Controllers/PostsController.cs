using System.Security.Claims;
using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Domain.Models.User;
using API.Domain.ModelView;
using API.Infrasctuture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[ApiController]
[Route("api/")]
public class PostsController(IPostService postService,
    UserManager<UserModel> userManager, AppDbContext context)
    : ControllerBase
{
    // Dependece Injection
    private readonly IPostService _postService = postService;
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly AppDbContext _context = context;

    [Authorize]
    [HttpPost("posts/")]
    public async Task<IActionResult> CreatePosts([FromBody] PostDTO postDTO)
    {
        // Find user loged by id
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

        // Visualization model to hide some data
        var result = new PostView
        {
            Title = postDTO.Title,
            Content = postDTO.Content,
            Author = user.UserName
        };

        await _postService.CreatePost(post);
        return Created($"Post created sucessfuly: ", result);
    }

    [HttpGet("posts")]
    public IActionResult ListPosts([FromQuery] int? page)
    {
        var query = _context.Posts.AsQueryable();

        int postsForPage = 10;

        // Pagination
        if (page != null && page > 0)
            query = query.Skip(((int)page - 1) * postsForPage).Take(postsForPage);

        // Filtring datas
        var result = query.Select(post => new
        {
            post.PostId,
            post.Title,
            post.Content,
            post.Username
        });

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePosts(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound("Post not found.");

        if (post.UserId != userId)
            return Forbid("You are not allowed to delete this post.");

        await _postService.DeletePost(post);

        return NoContent();
    }

    [Authorize]
    [HttpPut("posts")]
    public async Task<IActionResult> ModifyPosts([FromQuery] int id, PostDTO postDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound("Post not found.");

        if (post.UserId != userId)
            return Forbid("You are not allowed to modify this post.");

        // Changing the informations: title and content
        post.Title = postDTO.Title;
        post.Content = postDTO.Content;

        await _postService.UpdatePost(post);

        return Ok("Post successfully modified.");
    }
}
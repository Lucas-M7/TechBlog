using System.Drawing;
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
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly UserManager<UserModel> _userManager;
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostsController"/> class.
    /// </summary>
    /// <param name="postService">The post service for handling post operations.</param>
    /// <param name="userManager">The user manager for handling user operations.</param>
    /// <param name="context">The database context for accessing the database.</param>
    public PostsController(IPostService postService, UserManager<UserModel> userManager, AppDbContext context)
    {
        _postService = postService;
        _userManager = userManager;
        _context = context;
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="postDTO">The post data transfer object.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    [Authorize]
    [HttpPost("posts/")]
    public async Task<IActionResult> CreatePost([FromForm] PostDTO postDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        var post = new PostModel
        {
            Title = postDTO.Title,
            UserId = userId,
            Username = user.UserName
        };

        await _postService.CreatePost(post, postDTO.Image);

        var result = new PostView
        {
            Title = postDTO.Title,
            Content = postDTO.Content,
            ImagePath = post.ImagePath,
            Author = user.UserName
        };

        return CreatedAtAction(nameof(CreatePost), new { id = post.PostId }, result);
    }

    /// <summary>
    /// Lists all posts with optional pagination.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>An action result with the list of posts.</returns>
    [HttpGet("posts")]
    public IActionResult ListPosts([FromQuery] int? page)
    {
        const int postsPerPage = 10;

        var query = _context.Posts.AsQueryable();

        if (page.HasValue && page > 0)
            query = query.Skip((page.Value - 1) * postsPerPage).Take(postsPerPage);

        var result = query.Select(post => new
        {
            post.PostId,
            post.Title,
            post.Content,
            post.Username,
            ImageUrl = Url.Content($"~/{post.ImagePath}")
        });

        return Ok(result);
    }

    /// <summary>
    /// Deletes a post by ID.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
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

    /// <summary>
    /// Modifies an existing post.
    /// </summary>
    /// <param name="id">The ID of the post to modify.</param>
    /// <param name="postDTO">The post data transfer object with updated information.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    [Authorize]
    [HttpPut("posts")]
    public async Task<IActionResult> ModifyPost([FromQuery] int id, [FromBody] PostDTO postDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound("Post not found.");

        if (post.UserId != userId)
            return Forbid("You are not allowed to modify this post.");
            
        post.Title = postDTO.Title;
        post.Content = postDTO.Content;

        await _postService.UpdatePost(post);

        return Ok("Post successfully modified.");
    }
}
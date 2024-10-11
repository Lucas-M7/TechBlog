using System.Security.Claims;
using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Domain.Models.User;
using API.Domain.ModelView;
using API.Infrastructure.Data;
using API.Services.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[ApiController]
[Route("api/")]
[ServiceFilter(typeof(LogActionFilter))]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly UserManager<UserModel> _userManager;
    private readonly AppDbContext _context;
    private readonly ILogger<PostsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostsController"/> class.
    /// </summary>
    /// <param name="postService">The post service for handling post operations.</param>
    /// <param name="userManager">The user manager for handling user operations.</param>
    /// <param name="context">The database context for accessing the database.</param>
    public PostsController(IPostService postService, UserManager<UserModel> userManager, AppDbContext context, ILogger<PostsController> logger)
    {
        _postService = postService;
        _userManager = userManager;
        _context = context;
        _logger = logger;
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
            Content = postDTO.Content,
            UserId = userId,
            Username = user.UserName
        };

        _logger.LogInformation("Creating post.");
        await _postService.CreatePost(post);

        var result = new PostView
        {
            Title = postDTO.Title,
            Content = postDTO.Content,
            Author = user.UserName
        };

        _logger.LogInformation("Post created sucessfully.");
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
            post.Username
        });

        _logger.LogInformation("Posts listed sucessfully.");
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

        _logger.LogInformation("Post deleted sucessfully.");
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

        _logger.LogInformation("Post sucessfully modified.");
        return Ok("Post successfully modified.");
    }

    /// <summary>
    /// Funcionallity of search
    /// </summary>
    /// <param name="query"></param>
    /// <returns>Returns the posts that has the term</returns>
    [HttpGet("posts/search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Search query cannot be empty.");

        var queryPost = await _postService.SearchPostsAsync(query);

        var result = queryPost.Select(post => new
        {
            post.PostId,
            post.Title,
            post.Content,
            post.Username
        });

        return Ok(result);
    }
}
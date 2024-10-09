using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

/// <summary>
/// Service for managing posts.
/// And initializes a new instace of the <see cref="PostService"/> class.
/// </summary>
/// <param name="context">The application database context.</param>
public class PostService(AppDbContext context) : IPostService
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreatePost(PostModel post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="post">The post to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdatePost(PostModel post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="post">The post to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeletePost(PostModel post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the username in posts for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="newUsername">The new username to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateUsernameInPosts(string userId, string newUsername)
    {
        var posts = _context.Posts.Where(p => p.UserId == userId).ToList(); // Querying all posts of user by userId

        // Changin username in all posts of this user
        foreach (var post in posts)
        {
            post.Username = newUsername;
        }

        await _context.SaveChangesAsync();
    }
}
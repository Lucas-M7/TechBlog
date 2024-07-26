using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Infrasctuture.Data;

namespace API.Services;

public class PostService(AppDbContext context) : IPostService
{
    private readonly AppDbContext _context = context;

    public async Task CreatePost(PostModel post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePost(PostModel post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePost(PostModel post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }

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
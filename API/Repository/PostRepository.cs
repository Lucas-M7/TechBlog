using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PostModel>> SearchPostsAsync(string searchTerm)
    {
        return await _context.Posts
            .Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm))
            .ToListAsync();
    }
}
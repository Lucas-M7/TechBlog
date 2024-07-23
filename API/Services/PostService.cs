using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.Post;
using API.Infrasctuture.Data;

namespace API.Services;

public class PostService(AppDbContext context) : IPostService
{
    private readonly AppDbContext _context = context;

    public void CreatePost(PostModel post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }
}
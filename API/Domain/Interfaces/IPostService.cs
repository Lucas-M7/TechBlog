using API.Domain.DTOs;
using API.Domain.Models.Post;

namespace API.Domain.Interfaces;

public interface IPostService
{
    public void CreatePost(PostModel post);
}
using API.Domain.Models.Post;

namespace API.Domain.Interfaces;

public interface IPostService
{
    public Task CreatePost(PostModel post);
    public Task UpdatePost(PostModel post);
    public Task DeletePost(PostModel post);
    public Task UpdateUsernameInPosts(string userId, string newUsername);
}
using API.Domain.Models.Post;

namespace API.Domain.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<PostModel>> SearchPostsAsync(string searchTerm);
}
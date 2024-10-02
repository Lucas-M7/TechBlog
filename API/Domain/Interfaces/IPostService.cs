using API.Domain.Models.Post;

namespace API.Domain.Interfaces;

/// <summary>
/// Interface for post service operations.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreatePost(PostModel post, IFormFile file);

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="post">The post to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdatePost(PostModel post);

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="post">The post to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeletePost(PostModel post);

    /// <summary>
    /// Updates the username in posts for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="newUsername">The new username to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateUsernameInPosts(string userId, string newUsername);
}
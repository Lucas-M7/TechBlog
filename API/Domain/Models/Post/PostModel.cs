using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Models.Post;

/// <summary>
/// Model for the post.
/// </summary>
public class PostModel
{
    /// <summary>
    /// Gets or sets the post ID.
    /// </summary>
    [Key]
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(4, ErrorMessage = "The title must have at least 4 characters.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    [Required(ErrorMessage = "Content is required.")] 
    [MinLength(10, ErrorMessage = "The content must have at least 10 characters.")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user ID associated with the post.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username associated with the post.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user associated with the post.
    /// </summary>
    public virtual IdentityUser User { get; set; } = new IdentityUser();
}
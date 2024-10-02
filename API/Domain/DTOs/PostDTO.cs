using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

/// <summary>
/// DTO for create post.
/// </summary>
public class PostDTO
{
    /// <summary>
    /// Gets or sets the title of post.
    /// </summary>
    [Required, MinLength(4, ErrorMessage = "The title must have 4 charactes long.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Content of the post.
    /// </summary>
    [Required, MinLength(10, ErrorMessage = "The content must have 10 characters long.")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Image of the post.
    /// </summary>
    public IFormFile Image { get; set; } = default!;
}
namespace API.Domain.ModelView;

/// <summary>
/// Represents a view model for a post.
/// </summary>
public class PostView
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the author of the post.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;
}
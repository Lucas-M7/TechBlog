using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Models.Post;

public class PostModel
{
    [Key]
    public int PostId { get; set; }

    [Required(ErrorMessage = "Title is required."), 
        MinLength(4, ErrorMessage = "The title must have 4 characters long.")]
    public string Title { get; set; } = string.Empty;

    [Required, MinLength(10, ErrorMessage = "The content must have 10 characters long.")]
    public string Content { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = default!;
}
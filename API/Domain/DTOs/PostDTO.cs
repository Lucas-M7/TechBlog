using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

public class PostDTO
{
    [Required, MinLength(4, ErrorMessage = "The title must have 4 charactes long.")]
    public string Title { get; set; } = string.Empty;

    [Required, MinLength(10, ErrorMessage = "The content must have 10 characters long.")]
    public string Content { get; set; } = string.Empty;
}
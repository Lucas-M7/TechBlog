using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "The email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "The password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember-me?")]
    public bool RememberMe { get; set; }
}
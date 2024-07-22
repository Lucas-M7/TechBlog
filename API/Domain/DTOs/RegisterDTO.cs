using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

public class RegisterDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password), MinLength(7, ErrorMessage = "The password must have at least 7 characters long.")]
    public string Password { get; set; } = string.Empty;

    public int Age { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm your password.")]
    [Compare("Password", ErrorMessage = "Passwords are not the same.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
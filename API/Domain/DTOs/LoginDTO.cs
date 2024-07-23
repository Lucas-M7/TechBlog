using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "THe username is required.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "The password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember-me?")]
    public bool RememberMe { get; set; }
}
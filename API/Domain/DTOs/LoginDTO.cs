using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

/// <summary>
/// DTO for realize the login.
/// </summary>
public class LoginDTO
{
    /// <summary>
    /// Gets or sets the current username.
    /// </summary>
    [Required(ErrorMessage = "The username is required.")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current password.
    /// </summary>
    [Required(ErrorMessage = "The password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value indicating wheter the user wants to be remembered.
    /// </summary>
    [Display(Name = "Remember Me?")]
    public bool RememberMe { get; set; }
}
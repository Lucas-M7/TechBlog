using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

/// <summary>
/// DTO for the register the user.
/// </summary>
public class RegisterDTO
{
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [Required(ErrorMessage = "The email address is required.")]
    [EmailAddress(ErrorMessage = "The email address is not valid.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required(ErrorMessage = "The username is required.")]
    [MinLength(3, ErrorMessage = "The username must have at least 3 characters.")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required(ErrorMessage = "The password is required.")]
    [DataType(DataType.Password)]
    [MinLength(7, ErrorMessage = "The password must have at least 7 characters.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the age of the user.
    /// </summary>
    [Range(0, 120, ErrorMessage = "The age must be between 0 and 120.")]
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the confirmation password.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm your password.")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
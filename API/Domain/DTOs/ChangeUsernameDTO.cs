using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

/// <summary>
/// DTO for changing the username.
/// </summary>
public class ChangeUsernameDTO
{
    /// <summary>
    /// Gets or sets the new username.
    /// </summary>
    [Required(ErrorMessage = "New username is required.")]
    [StringLength(50, ErrorMessage = "New username cannot be longer than 50 characters.")]
    public string NewUsername { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current password.
    /// </summary>
    [Required(ErrorMessage = "Current password is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;
}
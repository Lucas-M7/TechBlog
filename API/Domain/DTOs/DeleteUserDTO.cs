using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

/// <summary>
/// DTO for delete user with the password.
/// </summary>
public class DeleteUserDTO
{   
    /// <summary>
    /// Gets or sets the current password.
    /// </summary>
    [Required(ErrorMessage = "Current password is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Models.User;

/// <summary>
/// Model for a user.
/// </summary>
public class UserModel : IdentityUser
{
    /// <summary>
    /// Gets or sets the age of the user.
    /// </summary>
    [Range(0, 120, ErrorMessage = "The age must be between 0 and 120.")]
    public int Age { get; set; }
}
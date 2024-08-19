using System.ComponentModel.DataAnnotations;

namespace API.Domain.DTOs;

public class DeleteUserDTO
{    
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
}
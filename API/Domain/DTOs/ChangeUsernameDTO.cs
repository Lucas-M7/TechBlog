namespace API.Domain.DTOs;

public class ChangeUsernameDTO
{
    public string NewUsername { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;
}
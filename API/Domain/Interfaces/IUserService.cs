using API.Domain.DTOs;

namespace API.Domain.Interfaces;

/// <summary>
/// Interface for user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="registerDTO">The registration data to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Register(RegisterDTO registerDTO);

    /// <summary>
    /// Login a the user.
    /// </summary>
    /// <param name="loginDTO">The login data of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Login(LoginDTO loginDTO);
}
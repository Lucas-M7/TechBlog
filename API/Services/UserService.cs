using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

/// <summary>
/// Service for managing user operations.
/// And initializes a new instance of the <see cref="UserService"> class.
/// </summary>
/// <param name="userManager">The user manager for handling user operations.</param>
/// <param name="signInManager">The sign-in manager for handling user sign-ins.</param>
public class UserService(UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager) : IUserService
{
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly SignInManager<UserModel> _signInManager = signInManager;

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerDTO">The registration data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Register(RegisterDTO registerDTO)
    {
        // Creating a new user object
        var user = new UserModel
        {
            UserName = registerDTO.Username,
            Email = registerDTO.Email,
            Age = registerDTO.Age
        };

        // Creating the user and hashing the password
        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors);
            throw new ArgumentException($"User registration failed: {errors}");
        }
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginDTO">The login data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Login(LoginDTO loginDTO)
    {
        var result = await _signInManager.PasswordSignInAsync(
            loginDTO.Username, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid login attempt.");
    }
}
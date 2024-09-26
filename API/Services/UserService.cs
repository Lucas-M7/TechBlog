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
        var user = CreateUserModel(registerDTO);

        // Creating the user and hashing the password
        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
            HandleRegistrationErrors(result);
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginDTO">The login data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Login(LoginDTO loginDTO)
    {
        var result = await SignInUser(loginDTO);

        if (!result.Succeeded)
            HandleLoginErrors();
    }

    /// <summary>
    /// Creates a UserModel from a RegisterDTO
    /// </summary>
    /// <param name="registerDTO">The registration data transfer object.</param>
    /// <returns>A UserModel object.</returns>
    private static UserModel CreateUserModel(RegisterDTO registerDTO)
    {
        return new UserModel
        {
            UserName = registerDTO.Username,
            Email = registerDTO.Email,
            Age = registerDTO.Age
        };
    }

    /// <summary>
    /// Handles errors that occur during user registration.
    /// </summary>
    /// <param name="result">The IdentityResult object containing error information.</param>
    /// <exception cref="ArgumentException">Thrown when user registration fails.</exception>
    private static void HandleRegistrationErrors(IdentityResult result)
    {
        var errors = string.Join(", ", result.Errors);
        throw new ArgumentException($"User registration failed: {errors}");
    }

    /// <summary>
    /// Signs in a user using the provided login data.
    /// </summary>
    /// <param name="loginDTO">The login data transfer object.</param>
    /// <returns>A task representing the asynchronous sign-in operation.</returns>
    private async Task<SignInResult> SignInUser(LoginDTO loginDTO)
    {
        return await _signInManager.PasswordSignInAsync(
            loginDTO.Username, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure: false);
    }

    /// <summary>
    /// Handles errors that occur during user login.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">Thrown when login attempt is invalid.</exception>
    private static void HandleLoginErrors()
    {
        throw new UnauthorizedAccessException("Inavlid login attempt.");
    }
}
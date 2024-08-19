using API.Domain.DTOs;
using API.Domain.Interfaces;
using API.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

public class UserService(UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager) : IUserService
{
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly SignInManager<UserModel> _signInManager = signInManager;

    public async Task Register(RegisterDTO registerDTO)
    {
        // Adding datas
        var user = new UserModel
        { UserName = registerDTO.Username, Email = registerDTO.Email, Age = registerDTO.Age };

        // Hashing password
        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
            throw new ArgumentException(result.Errors.ToString());
    }

    public async Task Login(LoginDTO loginDTO)
    {
        var result = await _signInManager.PasswordSignInAsync(
            loginDTO.Username, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid login attempt.");
    }
}
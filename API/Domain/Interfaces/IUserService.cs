using API.Domain.DTOs;

namespace API.Domain.Interfaces;

public interface IUserService
{
    public Task Register(RegisterDTO registerDTO);
    public Task Login(LoginDTO loginDTO);   
}
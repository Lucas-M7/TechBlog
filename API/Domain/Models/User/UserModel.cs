using API.Domain.Models.Post;
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Models.User;

public class UserModel : IdentityUser
{
    public int Age { get; set; }
}
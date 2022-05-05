using MyGarage.Interfaces;

namespace MyGarage.Models;

public class LoginModel : IUserCredentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}
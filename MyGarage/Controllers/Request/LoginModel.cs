using MyGarage.Interfaces;

namespace MyGarage.Controllers.Request;

public class LoginModel : IUserCredentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}
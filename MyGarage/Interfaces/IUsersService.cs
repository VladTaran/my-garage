using MyGarage.Controllers;

namespace MyGarage.Interfaces;

public interface IUsersService
{
    public Task<string?> GetToken(string email, string password);
    public Task<Guid> Register(UserCreateRequest user);
    public bool IsUniqueUser(string username);
}
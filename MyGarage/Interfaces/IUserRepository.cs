using MyGarage.Models;

namespace MyGarage.Interfaces;

public interface IUserRepository
{
    Task<List<User>> ListAll();
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<Guid> Create(User user);
    Task Update(Guid id, User user);
    Task Delete(Guid id);
}
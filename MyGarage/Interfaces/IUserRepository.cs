using MyGarage.Data.Model;

namespace MyGarage.Interfaces;

public interface IUserRepository
{
    Task<List<User>> ListAll();
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<User> GetByNickname(string name);
    Task<Guid> Create(User user);
    Task<bool> Update(Guid id, User user);
    Task Delete(Guid id);
}
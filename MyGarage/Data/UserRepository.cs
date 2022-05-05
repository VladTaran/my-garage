using MyGarage.Data.Context;
using MyGarage.Interfaces;
using MyGarage.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MyGarage.Data;

public class UserRepository : IUserRepository
{
    private readonly IDbContext _dbContext;

    public UserRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> ListAll()
    {
        return await _dbContext.Users.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<User> GetById(Guid id)
    {
        return await _dbContext.Users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _dbContext.Users.Find(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Guid> Create(User user)
    {
        await _dbContext.Users.InsertOneAsync(user);
        return user.Id;
    }

    public async Task Update(Guid id, User user)
    {
        await _dbContext.Users.ReplaceOneAsync(u => u.Id == id, user);
    }

    public async Task Delete(Guid id)
    {
        await _dbContext.Users.DeleteOneAsync(x => x.Id == id);
    }
}
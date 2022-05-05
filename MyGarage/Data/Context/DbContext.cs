using MyGarage.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MyGarage.Data.Context;

public interface IDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<Car> Cars { get; }
}

public class DbContext : IDbContext
{
    private readonly IMongoDatabase? _database;

    public DbContext(IOptions<Settings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        if (client != null)
            _database = client.GetDatabase(settings.Value.Database);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("users_dev");
    public IMongoCollection<Car> Cars => _database.GetCollection<Car>("cars_dev");
}
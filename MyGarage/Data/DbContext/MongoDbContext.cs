using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyGarage.Data.Model;
using MyGarage.Models;

namespace MyGarage.Data.DbContext;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase? _database;
    private readonly string _environment;

    public MongoDbContext(IOptions<Settings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        if (client != null)
        {
            _database = client.GetDatabase(settings.Value.Database);
            _environment = settings.Value.Environment;
        }

        CreateIndexes();
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>($"users_{_environment}");
    public IMongoCollection<Car> Cars => _database.GetCollection<Car>($"cars_{_environment}");

    private void CreateIndexes()
    {
        Users.Indexes.CreateMany(new List<CreateIndexModel<User>>{
            new (Builders<User>.IndexKeys.Ascending(x => x.Id)),
            new (Builders<User>.IndexKeys.Ascending(x => x.Name)),
            new (Builders<User>.IndexKeys.Ascending(x => x.Email))
        });
    }
}
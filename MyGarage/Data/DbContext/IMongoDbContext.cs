using MongoDB.Driver;
using MyGarage.Data.Model;
using MyGarage.Models;

namespace MyGarage.Data.DbContext;

public interface IMongoDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<Car> Cars { get; }
}
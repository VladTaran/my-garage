using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyGarage.Data;
using MyGarage.Data.DbContext;
using MyGarage.Data.Model;
using MyGarage.Interfaces;
using MyGarage.Models;
using NUnit.Framework;

namespace MyGarage.Tests.Repository;

[TestFixture]
public class UserRepositoryTest : IDisposable
{
    private IMongoDbContext _dbContext;
    private IUserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        var config = InitConfiguration();
        var options = config.Get<Settings>();

        var settings = Options.Create(new Settings
        {
            ConnectionString = options.ConnectionString,
            Database = options.Database,
            Environment = options.Environment
        });

        _dbContext = new MongoDbContext(settings);
        
        
        _userRepository = new UserRepository(_dbContext);
    }

    [Test]
    public async Task UpdateUserTest()
    {
        //Given

        var userId = await CreateUser(new User
        {
            Id = Guid.NewGuid(),
            Nickname = "TestV1",
            Email = "test@g.c",
            Name = "Name",
            Description = "Description",
            Phone = "0123456789",
            Gender = GenderEnum.Male,
            Surname = "surname",
            DriverExperience = 5,
            PhotoId = 11.ToGuid(),
            DateOfBirth = new DateTime(1999, 02, 01, 0, 0, 0).ToUniversalTime(),
            Address = new AddressInfo
            {
                City = "city",
                Country = "country"
            }
        });

        //When

        var userToUpdate = new User
        {
            Name = "newName",
            Description = "newDescription",
            Phone = "0987654321",
            Gender = GenderEnum.Female,
            Surname = "newSurname",
            DriverExperience = 10,
            PhotoId = 22.ToGuid(),
            DateOfBirth = new DateTime(2000, 03, 02, 0, 0, 0).ToUniversalTime(),
            Address = new AddressInfo
            {
                City = "newCity",
                Country = "newCountry"
            }
        };
        
        
        await _userRepository.Update(userId, userToUpdate);

        var updated = await _userRepository.GetById(userId);

        //Then
        
        Assert.AreEqual(userToUpdate.Name, updated.Name);
        Assert.AreEqual(userToUpdate.Surname, updated.Surname);
        Assert.AreEqual(userToUpdate.Description, updated.Description);
        Assert.AreEqual(userToUpdate.Gender, updated.Gender);
        Assert.AreEqual(userToUpdate.DriverExperience, updated.DriverExperience);
        Assert.AreEqual(userToUpdate.DateOfBirth, updated.DateOfBirth);
        Assert.AreEqual(userToUpdate.Phone, updated.Phone);
        Assert.AreEqual(userToUpdate.PhotoId, updated.PhotoId);
        Assert.AreEqual(userToUpdate.Address.City, updated.Address.City);
        Assert.AreEqual(userToUpdate.Address.Country, updated.Address.Country);
    }

    private async Task<Guid> CreateUser(User user)
    {
        return await _userRepository.Create(new User
        {
            Id = Guid.NewGuid(),
            Nickname = "TestV",
            Email = "test@g.c"
        });
    }

    private static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
    }

    public void Dispose()
    {
        _dbContext.Users.Database.DropCollection("users_dev");
    }
}
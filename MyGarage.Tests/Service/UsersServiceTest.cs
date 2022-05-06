using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyGarage.Controllers;
using MyGarage.Interfaces;
using MyGarage.Models;
using MyGarage.Services;
using NUnit.Framework;
using Moq;
using Rhino.Mocks;

namespace MyGarage.Tests.Service;

[TestFixture]
public class UsersServiceTest
{
    private Mock<IUserRepository> _userRepositoryMock;
    private IUsersService _usersService;
    private Mock<IPasswordHasher<User>> _passwordHasherMock;
    private Mock<IIdGenerator> _idGeneratorMock;
    private Mock<IOptions<AppSettings>> _appSettings;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _appSettings = new Mock<IOptions<AppSettings>>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _idGeneratorMock = new Mock<IIdGenerator>();

        _usersService = new UsersService(_userRepositoryMock.Object, _appSettings.Object, _passwordHasherMock.Object, _idGeneratorMock.Object);

    }

    [Test]
    public async Task TryToRegisterUser_Success()
    {
        //Given 
        
        var userRequest = new UserCreateRequest
        {
            Email = "mygarage@g.c",
            Username = "My Garage Username",
            Password = "mygaragepassword1!"
        };

        var expectedId = ToGuid(1);
        _idGeneratorMock.Setup(x => x.NewGuid()).Returns(expectedId);
        _userRepositoryMock
            .Setup(x => x.Create(Arg<User>.Is.Anything))
            .Returns(Task.FromResult(expectedId));

        _passwordHasherMock.Setup(x => x.HashPassword(Arg<User>.Is.Anything, Arg<string>.Is.Anything)).Returns(string.Empty);
        
        // When
        
        var id = await _usersService.Register(userRequest);
        
        //Then
        
        Assert.AreEqual(expectedId, id);
        
    }
    
    public static Guid ToGuid(int value)
    {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}
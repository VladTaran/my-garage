using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyGarage.Controllers;
using MyGarage.Interfaces;
using MyGarage.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MyGarage.Services;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public UsersService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<string?> GetToken(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        var checkPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!checkPassword)
        {
            return null;
        }

        var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecretKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.UserData, user.Id.ToString()),
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<Guid> Register(UserCreateRequest userRequest)
    {
        var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = userRequest.Email,
            Username = userRequest.Username,
            Password = encryptedPassword
        };
        
        await _userRepository.Create(user);
        return user.Id;
    }

    public bool IsUniqueUser(string username)
    {
        throw new NotImplementedException();
    }
}
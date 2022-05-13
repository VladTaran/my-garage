using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MyGarage.Controllers;
using MyGarage.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyGarage.Data.Model;

namespace MyGarage.Services;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;
    private readonly IPasswordHasher<User> _passwordHashService;
    private readonly IIdGenerator _idGenerator;

    public UsersService(
        IUserRepository userRepository,
        IOptions<AppSettings> appSettings,
        IPasswordHasher<User> passwordHashService,
        IIdGenerator idGenerator
        )
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _idGenerator = idGenerator;
        _appSettings = appSettings.Value;
    }

    public async Task<string?> GetToken(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        var passwordVerificationResult = _passwordHashService.VerifyHashedPassword(user, user.Password, password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new ApplicationException("Password Hash Verification failed");
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
        var userExistsEmailCheck = await CheckUserExists("email", userRequest.Email);
        if (userExistsEmailCheck)
        {
            throw new ApplicationException("User already exists");
        }
        
        var userExistsNicknameCheck = await CheckUserExists("nickname", userRequest.Nickname);
        if (userExistsNicknameCheck)
        {
            throw new ApplicationException("User already exists");
        }

        var user = new User
        {
            Id = _idGenerator.NewGuid(),
            Email = userRequest.Email,
            Nickname = userRequest.Nickname,
        };
        
        var hashedPassword = _passwordHashService.HashPassword(user, userRequest.Password);
        user.Password = hashedPassword;
        
        await _userRepository.Create(user);
        return user.Id;
    }

    public async Task<bool> CheckUserExists(string property, string value)
    {
        switch (property)
        {
            case "email":
            {
                var user = await _userRepository.GetByEmail(value);
                return user != null;
            }
            case "nickname":
            {
                var user = await _userRepository.GetByNickname(value);
                return user != null;
            }
            default:
                return false;
        }
    }
}
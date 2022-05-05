using Microsoft.AspNetCore.Http;
using MyGarage.Interfaces;
using MyGarage.Models;
using MyGarage.Services;
using Microsoft.AspNetCore.Mvc;

namespace MyGarage.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : AuthorizedApiController
{
    private readonly IUsersService _usersService;
    private readonly IUserRepository _userRepository;
    
    public AuthController(IUsersService usersService, IUserRepository userRepository) 
    {
        _usersService = usersService;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        return Ok(new List<string>
        {
            "Value1",
            "Value2"
        });
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody] LoginModel model)
    {
        var token = await _usersService.GetToken(model.Email, model.Password);

        var user = await _userRepository.GetByEmail(model.Email);

        return Ok(new
        {
            Token = token,
            ExpiresIn = 3600,
            UserId =  user.Id,
            UserName = user.Username
        });
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] UserCreateRequest request)
    {
        await _usersService.Register(request);
        return Created(
            "User has been created successfully", 
            StatusCodes.Status201Created);
    }
}

public class UserCreateRequest
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
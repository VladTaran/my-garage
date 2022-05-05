using MyGarage.Interfaces;
using MyGarage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyGarage.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICarsRepository _carsRepository;
    
    public UserController(IUserRepository userRepository, ICarsRepository carsRepository)
    {
        _userRepository = userRepository;
        _carsRepository = carsRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        var cars = await _carsRepository.GetByCreatedUserId(id);

        return Ok(new UserGarage
        {
            UserId = user.Id,
            Username = user.Username,
            Cars = cars
        });
    }
}
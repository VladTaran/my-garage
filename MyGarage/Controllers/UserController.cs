using AutoMapper;
using MyGarage.Interfaces;
using MyGarage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGarage.Common;
using MyGarage.Controllers.Request;
using MyGarage.Data.Model;

namespace MyGarage.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUsersService _usersService;
    private readonly ICarsRepository _carsRepository;
    private readonly IMapper _mapper;
    
    public UserController(
        IUserRepository userRepository,
        IUsersService usersService,
        ICarsRepository carsRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _usersService = usersService;
        _carsRepository = carsRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        var cars = await _carsRepository.GetByCreatedUserId(id);

        return Ok(new UserGarage
        {
            UserData = user,
            Cars = cars
        });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByName([FromQuery] string name)
    {
        var user = await _userRepository.GetByNickname(name);
        if (user == null)
        {
            return NotFound();
        }

        var cars = await _carsRepository.GetByCreatedUserId(user.Id);

        return Ok(new UserGarage
        {
            UserData = user,
            Cars = cars
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
       var isUpdated = await _userRepository.Update(id, user);

       if (isUpdated)
       {
           return Ok(isUpdated);
       }

       return NotFound();
    }
    
    [AllowAnonymous]
    [HttpGet("exists")]
    public async Task<IActionResult> CheckUserExists([FromQuery] CheckUserExistRequest request)
    {
        var isUserExists = await _usersService.CheckUserExists(request.Property, request.Value);
        return Ok(isUserExists);
    }
}
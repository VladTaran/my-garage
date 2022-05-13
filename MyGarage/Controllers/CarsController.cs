using MyGarage.Common;
using MyGarage.Controllers.Request;
using MyGarage.Interfaces;
using MyGarage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGarage.Data.Model;

namespace MyGarage.Controllers;

[Authorize]
[ApiController]
[Route("api/cars")]
public class CarsController : AuthorizedApiController
{
    private readonly ICarsRepository _repo;

    public CarsController(ICarsRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SearchOptions options)
    {
        var items = await _repo.Search(options);

        return Ok(new PageOf<Car>
        {
            Items = items,
            Total = items.Count
        });
        
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Car car)
    {
        if (CurrentUserId != null) car.CreatedBy = new Guid(CurrentUserId);

        var id = await _repo.Create(car);
        return CreatedAtAction(nameof(Get), new { id }, car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(Guid id, [FromBody] UpdateCarRequest request)
    {
        await _repo.Update(id,
            new Car
            {
                Id = id,
                Model = request.Model,
                Year = request.Year,
                ImageId = request.ImageId
            });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repo.Delete(id);
        return NoContent();
    }
}
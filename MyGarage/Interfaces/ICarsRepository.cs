using MyGarage.Data.Model;
using MyGarage.Models;

namespace MyGarage.Interfaces;

public interface ICarsRepository
{
    Task<IList<Car>> Search(SearchOptions options);
    Task<IList<Car>> ListAll();
    Task<Car> GetById(Guid id);
    Task<IList<Car>> GetByCreatedUserId(Guid id);
    Task<Guid> Create(Car car);
    Task Update(Guid id, Car user);
    Task Delete(Guid id);
}
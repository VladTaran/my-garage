using MyGarage.Data.Model;

namespace MyGarage.Models;

public class UserGarage
{
    public User UserData { get; set; }
    
    public IEnumerable<Car> Cars { get; set; }

}
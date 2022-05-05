namespace MyGarage.Models;

public class UserGarage
{
    public Guid UserId { get; set; }
    
    public string Username { get; set; }
    
    public IEnumerable<Car> Cars { get; set; }

}
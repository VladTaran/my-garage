using MyGarage.Data.Model;

namespace MyGarage.Controllers.Request;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string Phone { get; set; }
    
    public string Description { get; set; }
    
    public Guid PhotoId { get; set; }

    public int DriverExperience { get; set; }

    public GenderEnum Gender { get; set; }
    
    public string Country { get; set; }
    
    public string City { get; set; }
}
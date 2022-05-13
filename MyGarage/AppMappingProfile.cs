using AutoMapper;
using MyGarage.Controllers.Request;
using MyGarage.Data.Model;

namespace MyGarage;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<UpdateUserRequest, User>();
    }
}
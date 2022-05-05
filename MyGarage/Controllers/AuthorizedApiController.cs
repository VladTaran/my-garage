using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MyGarage.Controllers;

public class AuthorizedApiController : ControllerBase
{
    protected string? CurrentUserId => HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Bookify.Api.Controllers;


[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase 
{
    [HttpGet("test")]
    public IActionResult test() => Ok("this endpoint show protected content");

}

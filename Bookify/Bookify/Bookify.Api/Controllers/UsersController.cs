using Bookify.Application.User;
using Bookify.Application.User.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Bookify.Api.Controllers;


[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(ISender sender) : ControllerBase 
{
    [HttpGet("test")]
    public IActionResult Test() => Ok("this endpoint show protected content");

    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewUserLocallyFromKeyClockToken()
    {
        var user = UserInToken.Get(User);
        var command = new CreateUserCommand(user.Id,user.FirestName,user.LastName,user.Email);
        var result=  await  sender.Send(command);
        return Created();
    }
   
}



public class UserInToken
{
    private UserInToken(Guid id ,string? firestName, string? lastName, string? email)
    {
        Id = id;
        FirestName = firestName;
        LastName = lastName;
        Email = email;
    }

    public Guid Id { get; set; }
    public string? FirestName { get; private set; }
    public string? LastName { get;private set; }
    public string? Email { get; private set; }

   public  static UserInToken Get(ClaimsPrincipal user)
   {
        var sid = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
        var email = user.FindFirst(ClaimTypes.Email)?.Value?? user.FindFirst("email")?.Value;
        var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? user.FindFirst("given_name")?.Value;
        var lastName = user.FindFirst(ClaimTypes.Surname)?.Value?? user.FindFirst("family_name")?.Value;
        bool valid = Guid.TryParse(sid, out Guid id);
        if (!valid)
            throw new ApplicationException("Try Again Later");
        return new UserInToken(id,firstName,lastName,email);

   }
}

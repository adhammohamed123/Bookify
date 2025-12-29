using Bookify.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.User.Register
{

    public record CreateUserCommand(
       Guid Id,
       string FirestName,
       string LastName,
       string Email) : ICommand<Guid>;
}

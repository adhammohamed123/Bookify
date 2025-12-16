using Bookify.Domain.Abstractions;
using Bookify.Domain.User.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.User
{
    public sealed class User : Entity
    {
        private User(Guid id,FirstName firstName,LastName lastName,Email email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public  FirstName FirstName { get; private set; }

        

        public LastName LastName { get; private set; }
        public Email Email { get; private set; }

        public static Result<User> CreateNewUser(FirstName firstName,
            LastName lastName,
            Email email)
        {
            // add any business rules or validations here
            var user = new User(Guid.NewGuid(),firstName,lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id,user.Email.Value));
            return Result<User>.Success(user);
        }
    }
   
}

using Bookify.Domain.Abstractions;
using Bookify.Domain.User.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.User
{
    public sealed class UserModel : Entity
    {
        private UserModel(Guid id,FirstName firstName,LastName lastName,Email email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public  FirstName FirstName { get; private set; }

        

        public LastName LastName { get; private set; }
        public Email Email { get; private set; }

        public static Result<UserModel> CreateNewUser(FirstName firstName,
            LastName lastName,
            Email email)
        {
            // add any business rules or validations here
            var user = new UserModel(Guid.NewGuid(),firstName,lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id,user.Email.Value));
            return Result.Success(user);
        }
    }

    public class UserErrors
    {
        public static readonly Error UserNotFound = new(
            "User.NotFound",
            "The specified user was not found.");
    }

}

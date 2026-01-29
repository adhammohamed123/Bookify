using Bookify.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.UnitTest.User
{
    internal class UserData
    {

        public static FirstName FirstName = new("First name");
        public static LastName LastName = new("Last name");
        public static Email Email = Email.Create("test@test.com"); 

    }
}

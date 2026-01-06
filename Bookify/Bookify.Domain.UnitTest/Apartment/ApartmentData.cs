using Bookify.Domain.Apartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.UnitTest.Apartment
{
    internal class ApartmentData
    {
        public static Name Name = new("apartment name");
        public static Description Description = new("apartment desc..");
        public static Address Address = new("state","city","street","zipcode","country");

            
    }
}

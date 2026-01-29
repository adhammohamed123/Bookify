using Bookify.Domain.Apartment;
using Bookify.Domain.Shared;
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
        public static Money price = new(100,Currency.USD);
        public static Money CleanningFee = new(10, Currency.USD);
        public static ICollection<Amenity> amenities = new Amenity[] { Amenity.Wifi, Amenity.AirConditioning };

        public static ApartmentModel Create()=> ApartmentModel.Create(Name,Description,Address,price,CleanningFee,amenities);

    }
}

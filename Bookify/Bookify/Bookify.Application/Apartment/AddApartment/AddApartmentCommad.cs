using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartment.Dtos;
using Bookify.Domain.Apartment;
using Bookify.Domain.Shared;
using Bookify.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookify.Application.Apartment.AddApartment
{
    public record AddApartmentCommad(
         string Name,
         string Description,
         string State, 
         string City, 
         string Street, 
         string ZipCode, 
         string Country,
         Money Price,
         Money CleanningFee,
         ICollection<Amenity> Amenities
   ):ICommand<ApartmentDto>;
}

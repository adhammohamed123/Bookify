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
    public record AddApartmentCommad : ICommand<ApartmentDto>
    {
         public required string Name { get; init; }
         public required string Description { get; init; }
         public required string State { get; init; }
         public required string City {  get; init; }
         public required string Street {  get; init; }
         public required string ZipCode {  get; init; }
         public required string Country {  get; init; }
         public required Money Price { get; init; }
         public required Money CleanningFee {  get; init; }
         public required ICollection<Amenity> Amenities {  get; init; }
    }
}

using Bookify.Domain.Apartment;
using Bookify.Domain.Shared;

namespace Bookify.Application.Apartment.Dtos
{
    public record ApartmentDto
    {
        public Guid Id { get; set; }
        public required string Name { get;  set; }
        public required string Description { get;  set; }
        public required Address Address { get;  set; }
        public required Money Price { get;  set; }
        public required Money CleanningFee { get;  set; }
        public DateTime? LastBookedOnUtc { get;  set; }
        public required ICollection<Amenity> Amenities { get;  set; } 
    }
}

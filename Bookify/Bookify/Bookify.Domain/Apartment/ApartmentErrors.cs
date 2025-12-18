using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartment
{
    public static class ApartmentErrors
    {
        public static Error NoAmenities => new("Apartment.NoAmenities", "An apartment must have at least one amenity.");
        public static Error ApartmentNotFound => new("Apartment.NotFound", "The specified apartment was not found.");
    }

}

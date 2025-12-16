using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartment
{
    public static class ApartmentErrors
    {
        public static Error NoAmenities => new Error("Apartment.NoAmenities", "An apartment must have at least one amenity.");
    }

}

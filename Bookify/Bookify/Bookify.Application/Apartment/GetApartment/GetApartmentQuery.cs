using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartment.Dtos;

namespace Bookify.Application.Apartment.GetApartment
{

    public record GetApartmentQuery(Guid ApartmentId) : ICachedQuery<ApartmentDto>
    {
        public string CacheKey => $"Apartments:{ApartmentId}";

        public TimeSpan? Expiration => null;
    }
}

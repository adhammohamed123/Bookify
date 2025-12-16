using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartment.Events
{
    public record ApartmentCreatedDomainEvent(Guid ApartmentId) : IDomainEvent;

}

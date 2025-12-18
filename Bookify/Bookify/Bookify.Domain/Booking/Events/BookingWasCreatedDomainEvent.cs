using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking.Events
{
    
    public record BookingWasCreatedDomainEvent(Guid BookingId) : IDomainEvent;

    
}
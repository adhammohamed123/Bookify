using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking.Events
{
    public record BookingWasConfirmedDomainEvent(Guid BookingId) : IDomainEvent;

    
}
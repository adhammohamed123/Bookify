using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking.Events
{
    public record BookingWasRejectedDomainEvent(Guid BookingId) : IDomainEvent;

    
}
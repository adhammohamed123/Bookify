using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking.Events
{
    public record BookingWasCompletedDomainEvent(Guid Id) : IDomainEvent;
    
}
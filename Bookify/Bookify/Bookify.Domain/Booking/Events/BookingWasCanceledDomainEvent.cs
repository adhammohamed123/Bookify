using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking.Events
{
    public record BookingWasCanceledDomainEvent(Guid Id) : IDomainEvent;
    
}
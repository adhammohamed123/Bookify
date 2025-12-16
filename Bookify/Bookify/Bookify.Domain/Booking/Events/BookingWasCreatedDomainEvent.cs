using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking.Events
{
    
    public record BookingWasCreatedDomainEvent(Guid BookingId) : IDomainEvent;
    public record BookingWasConfirmedDomainEvent(Guid BookingId) : IDomainEvent;
    public record BookingWasRejectedDomainEvent(Guid BookingId) : IDomainEvent;

    
}
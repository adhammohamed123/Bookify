using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Review.Events
{
    public record ReviewCreatedDomainEvent(Guid Id) : IDomainEvent;
}
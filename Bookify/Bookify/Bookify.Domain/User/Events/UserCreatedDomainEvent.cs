using Bookify.Domain.Abstractions;

namespace Bookify.Domain.User.Events
{
    public record UserCreatedDomainEvent(Guid userId,string email) : IDomainEvent;
   
}

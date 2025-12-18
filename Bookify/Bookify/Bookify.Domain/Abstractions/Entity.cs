using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Bookify.Domain.Abstractions
{
    public abstract class Entity(Guid id) 
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        public Guid Id { get; init; } = id;
        public bool IsDeleted { get; private set; }
        public void Delete() => IsDeleted = true;
        public IReadOnlyCollection<IDomainEvent> GetDomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(IDomainEvent domainEvent)=>_domainEvents.Add(domainEvent);
        public void ClearDomainEvents()=>_domainEvents.Clear();
    }


   
}

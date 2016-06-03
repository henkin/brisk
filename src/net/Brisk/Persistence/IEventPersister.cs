using Brisk.Events;

namespace Brisk.Persistence
{
    public interface IEventPersister
    {
        void Add(DomainEvent domainEvent);
    }
}
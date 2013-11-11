using TemplateLibrary.Repository;

namespace TemplateLibrary.Events
{
    public interface IDomainEventRepository
    {
        void Add(DomainEvent domainEvent);
    }

    public class DomainEventRepository : MongoBase, IDomainEventRepository, IService
    {
        public DomainEventRepository() : base("Events") {}
        public void Add(DomainEvent domainEvent)
        {
            GetCollection(domainEvent.GetType()).Save(domainEvent);
        }
    }
}
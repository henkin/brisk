//using Brisk.Repository;
//using MongoDB.Driver;

//namespace Brisk.Events
//{
//    public interface IDomainEventRepository
//    {
//        void Add(DomainEvent domainEvent);
//    }

//    public class DomainEventRepository : MongoBase, IDomainEventRepository, IService
//    {
//        public DomainEventRepository() : base("Events") {}
//        public void Add(DomainEvent domainEvent)
//        {
//            GetCollection(domainEvent.GetType()).Save(domainEvent, WriteConcern.Unacknowledged);
//        }
//    }
//}
using Brisk.Events;
using MongoDB.Driver.Builders;

namespace Brisk.Repository
{
 
    public class MongoPersister : MongoBase, IHandle<PersistenceEvent>, IPersister, IService
    {
        public void Handle(PersistenceEvent args)
        {
            //Type entityType = Type.GetType(args.EntityType);
            //var set = this.Set(entityType);
            //var entity = JsonConvert.DeserializeObject(args.SerializedEntity, entityType);

            switch (args.PersistenceAction)
            {
                case PersistenceAction.Create: Add(args.Entity);
                    break;
                case PersistenceAction.Update: Update(args.Entity);
                    break;
                case PersistenceAction.Delete: Delete(args.Entity);
                    break;
            }
        }

        public void Add(Entity entity)
        {
            GetCollection(entity.GetType()).Save(entity);
        }

        public void Update(Entity entity)
        {
            GetCollection(entity.GetType()).Save(entity);
        }
        public void Delete(Entity entity)
        {
            GetCollection(entity.GetType()).Remove(Query.EQ("_id", entity.ID));
        }
    }
}
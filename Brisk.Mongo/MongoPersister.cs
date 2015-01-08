using System;
using Brisk.Persistence;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Brisk.Mongo
{
 
    public class MongoPersister : MongoBase, IPersister, IService
    {
        //public void Handle(PersistenceEvent args)
        //{
        //    //Type entityType = Type.GetType(args.EntityType);
        //    //var set = this.Set(entityType);
        //    //var entity = JsonConvert.DeserializeObject(args.SerializedEntity, entityType);

        //    switch (args.PersistenceAction)
        //    {
        //        case PersistenceAction.Create: Add(args.Entity);
        //            break;
        //        case PersistenceAction.Update: Update(args.Entity);
        //            break;
        //        case PersistenceAction.Delete: Delete(args.Entity);
        //            break;
        //    }
        //}

        public void Add(Entity entity)
        {
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
            GetCollection(entity.GetType()).Save(entity, WriteConcern.Unacknowledged);
        }

        public void Update(Entity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            GetCollection(entity.GetType()).Save(entity, WriteConcern.Unacknowledged);
        }

        public void Delete(Entity entity)
        {
            GetCollection(entity.GetType()).Remove(Query.EQ("_id", entity.ID), WriteConcern.Unacknowledged);
        }
    }
}
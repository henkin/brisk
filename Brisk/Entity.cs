using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Brisk
{
    public interface IIdentifiable
    {
        Guid ID { get; set; }
    }

    public interface IOwnable
    {
        Guid OwnerID { get; set; }
    }

    public class Entity : IIdentifiable
    {
        [BsonId]
        public Guid ID { get; set; }
        public Entity()
        {
            ID = Guid.NewGuid();
        }
    }
}
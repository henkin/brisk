using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Brisk
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }

    public interface IOwnable
    {
        Guid OwnerID { get; set; }
    }

    public class Entity : IIdentifiable
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }


    }
}
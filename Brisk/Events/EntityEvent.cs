using Brisk.Persistence;

namespace Brisk.Events
{
    public class Created<TEntity> : PersistenceEvent<TEntity> where TEntity : Entity
    {
        public Created(TEntity entity) : base(entity, PersistenceAction.Create)
        {
        }
    }

    public class Updated<TEntity> : PersistenceEvent<TEntity> where TEntity : Entity
    {
        public Updated(TEntity entity)
            : base(entity, PersistenceAction.Update)
        {
        }
    }

    public class Deleted<TEntity> : PersistenceEvent<TEntity> where TEntity : Entity
    {
        public Deleted(TEntity entity)
            : base(entity, PersistenceAction.Delete)
        {
        }
    }

    //public class Retrieve<TEntity> : Command<TEntity> { }

    public class EntityEvent<TEntity> : DomainEvent
    {
        public TEntity Entity { get; set; }

        public EntityEvent(TEntity entity)
        {
            Entity = entity;
        }
    }
}
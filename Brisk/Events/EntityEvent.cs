namespace Brisk.Events
{

    public class Created<TEntity> : EntityEvent<TEntity> { public Created(TEntity entity) : base(entity) { } }
    public class Updated<TEntity> : EntityEvent<TEntity> { public Updated(TEntity entity) : base(entity) { } }
    public class Deleted<TEntity> : EntityEvent<TEntity> { public Deleted(TEntity entity) : base(entity) { } }
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
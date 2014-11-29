brisk
=====
An simpler Event-Source framework for .NET/Mono

Overview
--------

The *idea* is that __you__ use it as a persistence library, but underneath it creates a stream of
events and a snapshot DB. You can later get at the events, replay them, etc.

Uses MongoDB, Redis and Autofac

    YourEntity : IIdentifiable
    public interface IIdentifiable { Guid ID { get; } }
    YourService : IService<YourEntity>

    Created<TEntity> : EntityEvent<TEntity>
    Updated<TEntity> : EntityEvent<TEntity>
    Deleted<TEntity> : EntityEvent<TEntity>

    public Task<EntityEvent<TEntity>> Commander.Create(Entity)
    
    Commander.Update(Entity)
    Commander.Delete<TEntity>(guid ID)

    Repo<TEntity>().Linqs()
   
    RepoLog<TEntity>().Revision(3);
    RepoLog<TEntity>().Revisions();

    public On(Action<Event> f); 
    On<TEntityEvent>(Func<EventHandler> f)

    

BriskNode has two Properties:
- Commander
- Eventer

### Commander
Supports Create, Update, Delete, Query

    - Create<TEntity>
    
    - Update<TEntity>
    
    - Delete<TEntity>
    
    - Get<TEntity>
    
    - Find<TEntity>
    
### Eventer

- FindAll()

- Find<TEntity>

- FindByEvent<TEntityEvent<TEntity>>


# Your domain Entity is

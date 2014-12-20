using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Brisk.Events;
using Brisk.Repository;

namespace Brisk
{
    public class CommandResult<TCommand>
    {
        public bool IsSuccess { get; set; }
        public TCommand Command { get; set; }

        public CommandResult(TCommand command)
        {
            Command = command;
        }
    }

    public class Create<TEntity> : Command<TEntity> { public Create(TEntity entity) : base(entity) { } }
    public class Update<TEntity> : Command<TEntity> { public Update(TEntity entity) : base(entity) { } }
    public class Delete<TEntity> : Command<TEntity> { public Delete(TEntity entity) : base(entity) { } }
    //public class Retrieve<TEntity> : Command<TEntity> { }

    public class Command<TEntity>
    {
        public TEntity Entity { get; set; }

        public Command(TEntity entity)
        {
            Entity = entity;
        }
    }

    public interface ICommander
    {
        CommandResult<Create<TEntity>> Create<TEntity>(TEntity entity) where TEntity : Entity;
    }

    public class Commander : ICommander, IService
    {
        public IEventService Eventer { get; set; }
        public IPersister Persister { get; set; }
        public EntityServiceFactory EntityServiceFactory { get; set; }
        public CommandResult<Create<TEntity>> Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            
            
            // call 'validate' on the service
            
            // call 'create' on the service

            // send "created" event. 

            var entityService = GetEntityService<TEntity>();

            entityService.Create(entity);

            var command = new Create<TEntity>(entity);

            Eventer.Raise<PersistenceEvent>(new Created<TEntity>(entity));

            Persister.Add(entity);
            
            return new CommandResult<Create<TEntity>>(command) { IsSuccess = true };
           
        }

        private IEntityService<TEntity> GetEntityService<TEntity>() where TEntity : Entity
        {
            var entityService = EntityServiceFactory.Create<TEntity>();//new EntityService<TEntity>();
            return entityService;
        }
    }

    public class EntityServiceFactory
    {
        private readonly IComponentContext _container;

        public EntityServiceFactory(IComponentContext container)
        {
            _container = container;

        }

        public IEntityService<T> Create<T>() where T : Entity
        {
            // http://peterspattern.com/generate-generic-factories-with-autofac/
            EntityService<T> entityService;
            
            _container.TryResolve(out entityService);
            if (entityService == null)
            {
                return new EntityService<T>() { EventService = _container.Resolve<IEventService>()};
            }
            else
                return entityService;

        }
    }
    
}

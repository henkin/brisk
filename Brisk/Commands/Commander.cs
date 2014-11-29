using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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
        Task<CommandResult<Create<TEntity>>> Create<TEntity>(TEntity entity) where TEntity : Entity;
    }

    public class Commander : ICommander, IService
    {
        public IEventService Eventer { get; set; }

        public async Task<CommandResult<Create<TEntity>>> Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            var entityService = GetEntityService<TEntity>();
            
            entityService.Create(entity);
            
            var command = new Create<TEntity>(entity);
            
            // call 'validate' on the service
            
            // call 'create' on the service

            // send "created" event. 

            var task = new Task<CommandResult<Create<TEntity>>>(() =>
                new CommandResult<Create<TEntity>>(command)
                );
            task.RunSynchronously();
            return await task;// task;
        }

        private IEntityService<TEntity> GetEntityService<TEntity>() where TEntity : Entity
        {
            // bleh, DI
            var entitySerivce = new EntityService<TEntity>();
            return entitySerivce;
        }
    }


    
}

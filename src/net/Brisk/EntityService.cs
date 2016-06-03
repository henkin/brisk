using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Brisk.Events;

namespace Brisk
{
    /// <summary>
    /// Default implementation for the EntityService
    /// </summary>
    public class EntityService<TEntity> : IService, IEntityService<TEntity> where TEntity : Entity
    {
        public IEventService EventService { get; set; }

        public virtual bool Validate(TEntity entity)
        {
            return true;
        }

        public virtual void Create(TEntity entity)
        {
            // throw exception if it didn't work. 
            if (!Validate(entity))
                throw new InvalidOperationException("Failed validation");
        }

        private void Raise(EntityEvent<TEntity> entityEvent)
        {
            EventService.Raise(entityEvent);
        }
    }

    public interface IEntityService<in TEntity> where TEntity : Entity
    {
        void Create(TEntity entity);

        bool Validate(TEntity entity);
    }
}
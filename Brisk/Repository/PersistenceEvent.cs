using System.Linq;
using Brisk.Events;
using Newtonsoft.Json;

namespace Brisk.Repository
{
    public enum PersistenceAction
    {
        Add,
        Update,
        Delete
    }

    public class PersistenceEvent : DomainEvent
    {
        public string EntityType { get; set; }
        public PersistenceAction PersistenceAction { get; set; }
        public Entity Entity { get; set; }

        public PersistenceEvent(Entity entity, PersistenceAction action)
        {
            //SerializedEntity = JsonConvert.SerializeObject(entity);
            Entity = entity;
            EntityType = entity.GetType().AssemblyQualifiedName;
            PersistenceAction = action;
        }

        public override string ToString()
        {
            return string.Format("* {0}: {1}\n{2}",
                PersistenceAction.ToString(),
                EntityType.Split(',').First().Split('.').Last(),
                JsonConvert.SerializeObject(Entity, Formatting.Indented)
                );
        }
    }

    public static class PersistenceEventExtensions
    {

        public static bool IsPersistenceEvent<T>(this DomainEvent domainEvent)
        {
            return (domainEvent is PersistenceEvent &&
                    (domainEvent as PersistenceEvent).EntityType == typeof(T).AssemblyQualifiedName);
        }

		public static T GetEntity<T>(this DomainEvent domainEvent) where T : class
		{
			return (((domainEvent as PersistenceEvent).Entity) as T);
		}
    }
}

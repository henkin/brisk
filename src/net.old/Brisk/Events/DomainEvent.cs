using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Brisk.Events
{
    public interface IDomainEvent {}
    public class DomainEvent : Entity, IDomainEvent
    {
        public DomainEvent()
        {
            CreatedAt = DateTime.UtcNow;
            Dispatches = new List<DomainEventDispatch>();
            ID = Guid.NewGuid();
        }
        public Guid ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DispatchedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public List<DomainEventDispatch> Dispatches { get; set; }

        public override string ToString()
        {
            return string.Format("* {0} ({1}): created {2} ms ago\n{3}", 
                this.GetType().Name, 
                ID,
                (DateTime.UtcNow - CreatedAt).Milliseconds,
                JsonConvert.SerializeObject(this, Formatting.Indented)
                );
        }

        //internal void Dispatch()
        //{
        //    throw new NotImplementedException();
        //}
    }

    public static class DomainEventExtensionMethods
    {
        public static IQueryable<DomainEvent> FindUnhandled(this IQueryable<DomainEvent> domainEvents)
        {
            return domainEvents.Where(d => d.CompletedAt == null && d.DispatchedAt == null);
        }
    }


}

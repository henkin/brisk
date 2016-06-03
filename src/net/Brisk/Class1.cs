using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brisk.Events;

namespace Brisk
{
    /*
     * Entities
     * Commands
     * DomainEvent
     * 
     * Commander
     *  Create
     *  Update
     *  Delete
     *  
     * Eventer
     *  Subscribe
     *  Raise
     *  
     * Repository
     *  List
     *  Find
     */
    
    public class Entity { }
    public class Command { }


    public interface IEventer { void RaiseLocally(DomainEvent domainEvent); void OnEvent(Action<DomainEvent> call); }

    public class Eventer : IEventer
    {
        public void RaiseLocally(DomainEvent domainEvent) {}
        public void OnEvent(Action<DomainEvent> call)
        {
            var domainEvent = new DomainEvent();
            call(domainEvent);
        }
    }

    public class Persister
    {
    }

    public class Repository
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Brisk.Persistence;
using Brisk.Persistence;

namespace Brisk.Events
{
    public interface IHandleEverything : IService
    {
        void Handle(DomainEvent domainEvent);
    }

    public interface IHandle<TDomainEvent> where TDomainEvent : DomainEvent
    {
        void Handle(TDomainEvent args);
    }

    public interface IEventService
    {
        Task Raise<TEvent>(TEvent eve) where TEvent : DomainEvent;
    }


    public class EventService : IEventService, IService
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private ILifetimeScope _scope;
        private IHandleEverything _globalHandler;
        private IEventPersister _eventPersister;
        private Thread _processEventsThreadLoop;

        public EventService(ILifetimeScope scope, IEventPersister eventPersister, IHandleEverything globalHandler = null)
        {
            _scope = scope;
            _eventPersister = eventPersister;
            _globalHandler = globalHandler;
        }

        public Task Raise<TEvent>(TEvent eventToDispatch) where TEvent : DomainEvent
        {
            _logger.Debug("Raised: {0}", eventToDispatch.GetType().Name);
            var task = new Task(() => DispatchEvent(eventToDispatch));
            task.Start();
            return task;
        }

        private void DispatchEvent(DomainEvent domainEvent)
        {
            _logger.Debug(domainEvent);
            var handlerType = typeof (IHandle<>).MakeGenericType(domainEvent.GetType());

            //if (domainEvent is PersistenceEvent)
            //    handlerType = typeof(IHandle<>).MakeGenericType(typeof(PersistenceEvent));

            var handlers = _scope.Resolve(typeof (IEnumerable<>).MakeGenericType(handlerType));
           
            if (handlers == null)
                throw new Exception();

            //if (!((handlers as IEnumerable<DomainEvent>).Any()))
            //    logger.Debug("no handlers found");

            var method = typeof (EventService).GetMethod("DispatchHandlers");
            var dispatchMethod = method.MakeGenericMethod(handlers.GetType(), domainEvent.GetType());
            dispatchMethod.Invoke(this, new[] {handlers, domainEvent});

            if (_globalHandler != null)
                _globalHandler.Handle(domainEvent);

            _eventPersister.Add(domainEvent);
        }

        public void DispatchHandlers<THandlers, TEvent>(THandlers handlers, TEvent domainEvent) 
            where THandlers : IEnumerable<IHandle<TEvent>> 
            where TEvent : DomainEvent
        {
            bool handled = true;
            domainEvent.DispatchedAt = DateTime.UtcNow;

            

            foreach (var handler in handlers)
            {
                var dispatch = new DomainEventDispatch(handler.GetType().Name);
                try
                {
                    handler.Handle(domainEvent);
                    dispatch.Complete();
                }
                catch (Exception e)
                {
                    handled = false;
                    _logger.Error("Dispatching event {0} failed: {1}", domainEvent, e.Message);
                    dispatch.Error(e);
                }

                domainEvent.Dispatches.Add(dispatch);
                domainEvent.DispatchedAt = DateTime.UtcNow;

                if (dispatch.IsSuccessful)
                    _logger.Debug(dispatch);
                else
                    _logger.Error(dispatch);
            }

            if (handled)
                domainEvent.CompletedAt = DateTime.Now;

        }

        
        //public void Dispose()
        //{
        //    if (_processEventsThreadLoop != null && _processEventsThreadLoop.IsAlive)
        //        _processEventsThreadLoop.Abort();
        //}

        //#endregion
    }
}
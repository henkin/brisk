using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using System.Threading;

namespace TemplateLibrary.Events
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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private ILifetimeScope _scope;
        private IHandleEverything _globalHandler;
        private IDomainEventRepository _persister;
        private Thread _processEventsThreadLoop;

        public EventService(ILifetimeScope scope, IDomainEventRepository persister, IHandleEverything globalHandler = null)
        {
            _scope = scope;
            _persister = persister;
            _globalHandler = globalHandler;
        }

        public Task Raise<TEvent>(TEvent eventToDispatch) where TEvent : DomainEvent
        {
            logger.Debug("Raised: {0}", eventToDispatch.GetType().Name);
            var task = new Task(() => DispatchEvent(eventToDispatch));
            task.Start();
            return task;
        }

        private void DispatchEvent(DomainEvent domainEvent)
        {
            logger.Debug(domainEvent);
            var handlerType = typeof (IHandle<>).MakeGenericType(domainEvent.GetType());
            var handlers = _scope.Resolve(typeof (IEnumerable<>).MakeGenericType(handlerType));
           
            var method = typeof (EventService).GetMethod("DispatchHandlers");
            var dispatchMethod = method.MakeGenericMethod(handlers.GetType(), domainEvent.GetType());
            dispatchMethod.Invoke(this, new[] {handlers, domainEvent});

            if (_globalHandler != null)
                _globalHandler.Handle(domainEvent);
        }

        public void DispatchHandlers<THandlers, TEvent>(THandlers handlers, TEvent domainEvent) 
            where THandlers : IEnumerable<IHandle<TEvent>> 
            where TEvent : DomainEvent
        {
            bool handled = true;
            domainEvent.DispatchedAt = DateTime.UtcNow;

            if (!handlers.Any())
                logger.Debug("no handlers found");

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
                    logger.Error("Dispatching event {0} failed: {1}", domainEvent, e.Message);
                    dispatch.Error(e);
                }

                domainEvent.Dispatches.Add(dispatch);
                domainEvent.DispatchedAt = DateTime.UtcNow;

                if (dispatch.IsSuccessful)
                    logger.Debug(dispatch);
                else
                    logger.Error(dispatch);
            }

            if (handled)
                domainEvent.CompletedAt = DateTime.Now;

            _persister.Add(domainEvent);
        }

        public class Chainer
        {
            
        }
        //#region IDisposable Members

        //public void Dispose()
        //{
        //    if (_processEventsThreadLoop != null && _processEventsThreadLoop.IsAlive)
        //        _processEventsThreadLoop.Abort();
        //}

        //#endregion
    }
}
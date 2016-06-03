using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Brisk.Events;
using Brisk.Persistence;
using Brisk.RavenDB;

namespace Brisk.Application
{
    public interface IApp
    {
        ICommander Commander { get; set; }
        IEventService Eventer { get; set; }
        IRepository Repository { get; set; }
    }
    /// <summary>
    /// sometimes you have an app.
    /// sometimes you have to inject. 
    /// 
    /// if you have app, then just register with context
    /// 
    /// if not, instantiate instance, 
    /// </summary>
    public class App : IApp
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private ILifetimeScope Container { get; set; }
        public ICommander Commander { get; set; }
        public IEventService Eventer { get; set; }
        public IRepository Repository { get; set; }

        
        public static IApp Create(Type persisterType = null, params Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();
            
            RegisterApp(builder);
            RegisterPersister(builder, persisterType);

            // todo: unit-of-work containers
            var container = builder.Build();

            var services = container.ComponentRegistry.Registrations
                .SelectMany(x => x.Services)
                .OfType<IServiceWithType>()
                .ToList();

            services.ForEach(s => _logger.Debug(s));

            var app = container.Resolve<IApp>();

            return app;
        }

        private static void RegisterPersister(ContainerBuilder builder, Type persisterType)
        {
            // if nothing specified, default to Brisket DB
            if (persisterType == null)
                persisterType = typeof (RavenPersisterRepository);

            builder.RegisterType(persisterType).As<IPersister>().InstancePerMatchingLifetimeScope();
            builder.RegisterType(persisterType).As<IRepository>().InstancePerMatchingLifetimeScope();
        }

        internal void Start()
        {
            //throw new System.NotImplementedException();
        }

        private static void RegisterApp(ContainerBuilder builder, params Assembly[] assemblies)
        {
            AppDomain appDomain = AppDomain.CurrentDomain;
            IEnumerable<Assembly> scanAssemblies = assemblies.Length > 0 ? assemblies : appDomain.GetAssemblies();

            foreach (Assembly asm in scanAssemblies)
            {
                builder.RegisterType<EntityServiceFactory>().PropertiesAutowired().SingleInstance();
                builder.RegisterGeneric(typeof(EntityService<>)).PropertiesAutowired().InstancePerDependency();

                // Register Services
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => typeof(IService).IsAssignableFrom(t)
                                && t.IsClass
                                && !t.IsAbstract)
                    .AsImplementedInterfaces()
                    .PropertiesAutowired()
                    .InstancePerDependency();

                // Register Singletons
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => (
                        typeof(IEventService).IsAssignableFrom(t) ||
                        typeof(IApp).IsAssignableFrom(t)
                        )
                                && t.IsClass
                                && !t.IsAbstract)
                    .AsImplementedInterfaces()
                    .PropertiesAutowired()
                    .SingleInstance();
            }
        }
    }

    public static class ContainerBuilderExtensions
    {
    }
}
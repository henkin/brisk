using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Brisk.Events;
using Brisk.Persistence;

namespace Brisk
{
    public interface IApplication
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
    public class Application : IApplication
    {
        public ILifetimeScope Container { get; set; }
        public ICommander Commander { get; set; }
        public IEventService Eventer { get; set; }
        public IRepository Repository { get; set; }

        
        public static IApplication Create(Type persisterType = null, params Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();
            
            RegisterApp(builder);
            RegisterPersister(builder, persisterType);

            // todo: unit-of-work containers
            var container = builder.Build();
            var app = container.Resolve<IApplication>();


            return app;
        }

        private static void RegisterPersister(ContainerBuilder builder, Type persisterType)
        {
            // if nothing specified, default to Brisket DB
            if (persisterType == null)
                throw new ArgumentNullException("persisterType");

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
                        typeof(IApplication).IsAssignableFrom(t)
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
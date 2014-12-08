using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Brisk.Events;
using Brisk.Repository;

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

        public static IApplication Create(params Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApp();

            // todo: unit-of-work containers
           
            var container = builder.Build();
            var app = container.Resolve<IApplication>();


            return app;
        }

        internal void Start()
        {
            //throw new System.NotImplementedException();
        }
    }

    public static class ContainerBuilderExtensions
    {

        public static void RegisterApp(this ContainerBuilder builder, params Assembly[] assemblies)
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
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Brisk.Events;

namespace Brisk
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterApp(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            AppDomain appDomain = AppDomain.CurrentDomain;
            IEnumerable<Assembly> scanAssemblies = assemblies.Length > 0 ? assemblies : appDomain.GetAssemblies();

            foreach (Assembly asm in scanAssemblies)
            {
                // register services
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => typeof(IService).IsAssignableFrom(t)
                        && t.IsClass
                        && !t.IsAbstract)
                    .AsImplementedInterfaces()
                    .PropertiesAutowired()
                    .InstancePerDependency();

                // register services
                builder.RegisterAssemblyTypes(asm)
                    .Where(t => typeof(IEventService).IsAssignableFrom(t)
                        && t.IsClass
                        && !t.IsAbstract)
                    .AsImplementedInterfaces()
                    .PropertiesAutowired()

                    .SingleInstance();
            }
        }
    }
}

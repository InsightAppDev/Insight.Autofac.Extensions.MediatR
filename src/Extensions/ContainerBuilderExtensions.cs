using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Insight.Autofac.Extensions.MediatR
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddMediator(this ContainerBuilder builder, params string[] assemblies)
        {
            var loadedAssemblies = assemblies
                .Select(Assembly.Load)
                .ToArray();

            return AddMediator(builder, loadedAssemblies);
        }

        public static ContainerBuilder AddMediator(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterModule(new MediatorModule(assemblies));

            return builder;
        }

        public static ContainerBuilder AddMediator(this ContainerBuilder builder,
            IEnumerable<Assembly> assemblies, bool registerLoggingModule, bool registerValidationModule)
        {
            builder.RegisterModule(new MediatorModule(assemblies as Assembly[] ?? assemblies.ToArray(), registerLoggingModule, registerValidationModule));

            return builder;
        }
    }
}
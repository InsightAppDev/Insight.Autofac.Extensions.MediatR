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
            return builder.AddMediator(assemblies, false, false);
        }

        public static ContainerBuilder AddMediator(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return builder.AddMediator(assemblies, false, false);
        }

        public static ContainerBuilder AddMediator(this ContainerBuilder builder,
            IEnumerable<string> assemblies, bool registerLoggingModule, bool registerValidationModule)
        {
            return builder.AddMediator(assemblies.Select(Assembly.Load), registerLoggingModule,
                registerValidationModule);
        }

        public static ContainerBuilder AddMediator(this ContainerBuilder builder,
            IEnumerable<Assembly> assemblies, bool registerLoggingModule, bool registerValidationModule)
        {
            builder.RegisterModule(new MediatorModule(assemblies as Assembly[] ?? assemblies.ToArray(),
                registerLoggingModule, registerValidationModule));

            return builder;
        }

        public static ContainerBuilder AddMediatorValidation(this ContainerBuilder builder, params string[] assemblies)
        {
            return builder.AddMediatorValidation(assemblies.Select(Assembly.Load).ToArray());
        }

        public static ContainerBuilder AddMediatorValidation(this ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            builder.RegisterModule(new MediatorValidationModule(assemblies));

            return builder;
        }
        
        public static ContainerBuilder AddMediatorLogging(this ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorLoggingModule());

            return builder;
        }
    }
}
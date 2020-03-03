using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Insight.Autofac.Extensions.MediatR
{
    internal sealed class MediatorModule : Module
    {
        public Assembly[] Assemblies { get; }

        public bool RegisterLoggingModule { get; }

        public bool RegisterValidationModule { get; }

        internal MediatorModule(params Assembly[] assemblies)
        {
            Assemblies = assemblies;
        }

        internal MediatorModule(Assembly[] assemblies, bool registerLoggingModule, bool registerValidationModule) :
            this(assemblies)
        {
            RegisterLoggingModule = registerLoggingModule;
            RegisterValidationModule = registerValidationModule;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assemblies)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(Assemblies)
                .AsClosedTypesOf(typeof(IRequestExceptionHandler<,>));

            builder.RegisterAssemblyTypes(Assemblies)
                .AsClosedTypesOf(typeof(IRequestExceptionAction<,>));

            builder.RegisterAssemblyTypes(Assemblies)
                .AsClosedTypesOf(typeof(INotificationHandler<>));


            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            if (RegisterLoggingModule)
                builder.RegisterModule(new MediatorLoggingModule());

            if (RegisterValidationModule)
                builder.RegisterModule(new MediatorValidationModule(Assemblies));

            builder.Register<ServiceFactory>(outer =>
            {
                var inner = outer.Resolve<IComponentContext>();
                return serviceType => inner.Resolve(serviceType);
            });
        }
    }
}
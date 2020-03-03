using Autofac;
using MediatR;

namespace Insight.Autofac.Extensions.MediatR
{
    internal sealed class MediatorLoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGenericDecorator(typeof(RequestLoggingDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
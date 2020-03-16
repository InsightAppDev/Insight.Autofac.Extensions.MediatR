using Autofac;
using Insight.Autofac.Extensions.MediatR.Samples.Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Insight.Autofac.Extensions.MediatR.Tests
{
    public class ExtensionsTest
    {
        [Fact]
        public void Should_register_handlers()
        {
            var container = new ContainerBuilder()
                .AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application")
                .Build();

            var mediator = container.Resolve<IMediator>();
            Assert.NotNull(mediator);
            
            var handler = container.Resolve<IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>>();
            Assert.NotNull(handler);
        }

        [Fact]
        public void Should_register_logging_decorator()
        {
            var containerBuilder = new ContainerBuilder()
                .AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application")
                .AddMediatorLogging();

            containerBuilder
                .RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

            containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            var container = containerBuilder.Build();

            var mediator = container.Resolve<IMediator>();
            Assert.NotNull(mediator);

            var handler = container.Resolve<IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>>();
            Assert.NotNull(handler);
            Assert.Equal("RequestLoggingDecorator`2", handler.GetType().Name);
        }


        [Fact]
        public void Should_register_validation_decorator()
        {
            var containerBuilder = new ContainerBuilder()
                .AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application")
                .AddMediatorValidation("Insight.Autofac.Extensions.MediatR.Samples.Application");

            containerBuilder
                .RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

            containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            var container = containerBuilder.Build();

            var mediator = container.Resolve<IMediator>();
            Assert.NotNull(mediator);

            var handler = container.Resolve<IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>>();
            Assert.NotNull(handler);
            Assert.Equal("RequestValidationDecorator`2", handler.GetType().Name);
        }
    }
}

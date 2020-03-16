using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Insight.Autofac.Extensions.MediatR.Samples.Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Insight.Autofac.Extensions.MediatR.Tests
{
    public sealed class ExtensionsTest
    {
        [Fact]
        public void Should_throw_exception_on_register_handlers_by_assembly_if_assembly_is_empty()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new ContainerBuilder()
                    .AddMediator(new Assembly[] { })
                    .Build();
            });
        }

        [Fact]
        public void Should_register_handlers_by_assembly_name()
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
        public void Should_register_handlers_by_assembly()
        {
            var container = new ContainerBuilder()
                .AddMediator(Assembly.Load("Insight.Autofac.Extensions.MediatR.Samples.Application"))
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

            var loggingHandler = container.Resolve<IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>>();
            Assert.NotNull(loggingHandler);
            Assert.Equal("RequestLoggingDecorator`2", loggingHandler.GetType().Name);

            var target = loggingHandler.GetType().GetField("_inner", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(loggingHandler);
            Assert.NotNull(target);
            Assert.Equal(typeof(GetCurrentUtcDateTimeStringHandler), target.GetType());
        }

        [Fact]
        public void Should_register_validation_decorator()
        {
            var containerBuilder = new ContainerBuilder()
                .AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application")
                .AddMediatorValidation("Insight.Autofac.Extensions.MediatR.Samples.Application");

            var container = containerBuilder.Build();

            var mediator = container.Resolve<IMediator>();
            Assert.NotNull(mediator);

            var validationHandler = container.Resolve<IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>>();
            Assert.NotNull(validationHandler);
            Assert.Equal("RequestValidationDecorator`2", validationHandler.GetType().Name);

            var target = validationHandler.GetType().GetField("_inner", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(validationHandler);
            Assert.NotNull(target);
            Assert.Equal(typeof(GetCurrentUtcDateTimeStringHandler), target.GetType());
        }

        [Fact]
        public void Should_throw_request_validation_exception()
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

            Assert.ThrowsAsync<RequestValidationException>(async () => await mediator.Send(new GetCurrentUtcDateTimeStringQuery(null)));
        }

        [Fact]
        public async Task Should_register_logging_and_validation_decorators()
        {
            var containerBuilder = new ContainerBuilder()
                .AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application")
                .AddMediatorValidation("Insight.Autofac.Extensions.MediatR.Samples.Application")
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

            var loggingHandler = container.Resolve<IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>>();
            Assert.NotNull(loggingHandler);
            Assert.Equal("RequestLoggingDecorator`2", loggingHandler.GetType().Name);

            var validationHandler = loggingHandler.GetType().GetField("_inner", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(loggingHandler);
            Assert.NotNull(validationHandler);
            Assert.Equal("RequestValidationDecorator`2", validationHandler.GetType().Name);

            var target = validationHandler.GetType().GetField("_inner", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(validationHandler);
            Assert.NotNull(target);
            Assert.Equal(typeof(GetCurrentUtcDateTimeStringHandler), target.GetType());

            var result = await mediator.Send(new GetCurrentUtcDateTimeStringQuery("yyyy-MM-dd"), CancellationToken.None);
            Assert.NotEmpty(result);
            Assert.Equal($"{DateTime.UtcNow:yyyy-MM-dd}", result, StringComparer.InvariantCultureIgnoreCase);

        }
    }
}
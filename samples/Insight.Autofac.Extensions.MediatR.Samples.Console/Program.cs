using System.Threading.Tasks;
using Autofac;
using Insight.Autofac.Extensions.MediatR.Samples.Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Insight.Autofac.Extensions.MediatR.Samples.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SimpleSample();

            await SampleWithValidation();

            await LoggingSample();
        }

        private static async Task SampleWithValidation()
        {
            var containerBuilder = BuildSimpleContainer();
            containerBuilder.AddMediatorValidation("Insight.Autofac.Extensions.MediatR.Samples.Application");
            var container = containerBuilder.Build();

            var mediatR = container.Resolve<IMediator>();

            var dateTime = await mediatR.Send(new GetCurrentUtcDateTimeStringQuery("MM-dd-yyyy"));

            System.Console.WriteLine($"Current datetime is: {dateTime}");
        }

        private static async Task LoggingSample()
        {
            var containerBuilder = BuildSimpleContainer();

            containerBuilder
                .RegisterInstance(LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter("Insight.Autofac.Extensions.MediatR.Samples.Application", LogLevel.Information)
                        .SetMinimumLevel(LogLevel.Debug)
                        .AddConsole();
                }))
                .As<ILoggerFactory>();

            containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            containerBuilder.AddMediatorLogging();

            var container = containerBuilder.Build();

            var mediatR = container.Resolve<IMediator>();

            var dateTime = await mediatR.Send(new GetCurrentUtcDateTimeStringQuery("MM-dd-yyyy"));

            System.Console.WriteLine($"Current datetime is: {dateTime}");
        }

        private static async Task SimpleSample()
        {
            var simpleContainer = BuildSimpleContainer().Build();

            var mediatR = simpleContainer.Resolve<IMediator>();

            var dateTime = await mediatR.Send(new GetCurrentUtcDateTimeStringQuery("MM-dd-yyyy"));

            System.Console.WriteLine($"Current datetime is: {dateTime}");
        }

        private static ContainerBuilder BuildSimpleContainer() =>
            new ContainerBuilder().AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application");
    }
}
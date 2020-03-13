using System.Threading.Tasks;
using Autofac;
using Insight.Autofac.Extensions.MediatR.Samples.Application.Queries;
using MediatR;

namespace Insight.Autofac.Extensions.MediatR.Samples.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SimpleSample();

            await SampleWithValidation();
        }

        private static async Task SampleWithValidation()
        {
            var simpleContainer = BuildContainerWithValidation();

            var mediatR = simpleContainer.Resolve<IMediator>();

            var dateTime = await mediatR.Send(new GetCurrentUtcDateTimeStringQuery("MM-dd-yyyy"));

            System.Console.WriteLine($"Current datetime is: {dateTime}");
        }

        private static async Task SimpleSample()
        {
            var simpleContainer = BuildSimpleContainer();

            var mediatR = simpleContainer.Resolve<IMediator>();

            var dateTime = await mediatR.Send(new GetCurrentUtcDateTimeStringQuery("MM-dd-yyyy"));

            System.Console.WriteLine($"Current datetime is: {dateTime}");
        }

        private static IContainer BuildSimpleContainer()
        {
            var builder = new ContainerBuilder();

            builder.AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application");

            return builder.Build();
        }

        private static IContainer BuildContainerWithValidation()
        {
            var builder = new ContainerBuilder();

            builder.AddMediator(new[] {"Insight.Autofac.Extensions.MediatR.Samples.Application"}, false, true);

            return builder.Build();
        }
    }
}
using MediatR;

namespace Insight.Autofac.Extensions.MediatR.Samples.Application.Queries
{
    public sealed class GetCurrentUtcDateTimeStringQuery : IRequest<string>
    {
        public GetCurrentUtcDateTimeStringQuery(string format)
        {
            Format = format;
        }

        public string Format { get; private set; }
    }
}
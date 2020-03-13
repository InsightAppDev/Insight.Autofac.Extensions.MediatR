using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Insight.Autofac.Extensions.MediatR.Samples.Application.Queries
{
    public sealed class GetCurrentUtcDateTimeStringHandler : IRequestHandler<GetCurrentUtcDateTimeStringQuery, string>
    {
        public Task<string> Handle(GetCurrentUtcDateTimeStringQuery request, CancellationToken cancellationToken) =>
            Task.FromResult(DateTimeOffset.UtcNow.ToString(request.Format));
    }
}
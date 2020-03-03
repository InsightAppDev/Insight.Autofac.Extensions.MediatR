using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Insight.Autofac.Extensions.MediatR
{
    internal sealed class RequestLoggingDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, IHandlerDecorator
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RequestLoggingDecorator<TRequest, TResponse>> _logger;

        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public RequestLoggingDecorator(IRequestHandler<TRequest, TResponse> inner,
            ILogger<RequestLoggingDecorator<TRequest, TResponse>> logger)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _inner = inner;
            _logger = logger;
        }

        [DebuggerStepThrough]
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var requestType = request
                .GetType();

            var type = _inner
                .GetHandlerType();

            try
            {
                _logger.LogTrace("{type} Executing {name}:\r\n{@request}", type, requestType.Name, request);

                var response = await _inner.Handle(request, cancellationToken);
                if (response == null)
                    _logger.LogTrace("{type} Request {request} returned null", type, requestType.Name);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{type} Error at {name}", type, requestType.Name);

                throw;
            }
        }

        public Type GetHandlerType()
        {
            return _inner.GetHandlerType();
        }
    }
}
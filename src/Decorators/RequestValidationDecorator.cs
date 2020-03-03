using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Insight.Autofac.Extensions.MediatR
{
    internal sealed class RequestValidationDecorator<TRequest, TResponse> :
        IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<AbstractValidator<TRequest>> _validators;
        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public RequestValidationDecorator(IEnumerable<AbstractValidator<TRequest>> validators,
            IRequestHandler<TRequest, TResponse> inner)
        {
            _validators = validators;
            _inner = inner;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken token)
        {
            await Validate(request, token);

            return await _inner.Handle(request, token);
        }

        private async Task Validate(TRequest request, CancellationToken token)
        {
            if (_validators != null)
            {
                foreach (var validator in _validators)
                {
                    var result = await validator.ValidateAsync(request, token);
                    if (!result.IsValid)
                        throw new RequestValidationException(result.ToString());
                }
            }
        }
    }
}

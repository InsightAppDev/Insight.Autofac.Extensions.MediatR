using FluentValidation;

namespace Insight.Autofac.Extensions.MediatR.Samples.Application.Queries
{
    public sealed class GetCurrentUtcDateTimeStringValidator : AbstractValidator<GetCurrentUtcDateTimeStringQuery>
    {
        public GetCurrentUtcDateTimeStringValidator()
        {
            RuleFor(c => c)
                .NotEmpty()
                .WithMessage($"Query is empty: {nameof(GetCurrentUtcDateTimeStringQuery)}");

            RuleFor(c => c.Format)
                .NotEmpty()
                .WithMessage($"DateTime format is empty: {nameof(GetCurrentUtcDateTimeStringQuery.Format)}");
        }
    }
}
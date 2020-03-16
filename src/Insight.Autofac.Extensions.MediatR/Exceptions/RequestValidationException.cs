using System;

namespace Insight.Autofac.Extensions.MediatR
{
    public sealed class RequestValidationException : Exception
    {
        internal RequestValidationException(string message) : base(message)
        {
        }
    }
}
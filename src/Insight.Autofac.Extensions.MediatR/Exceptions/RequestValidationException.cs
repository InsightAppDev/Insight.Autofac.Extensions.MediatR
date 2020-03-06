using System;

namespace Insight.Autofac.Extensions.MediatR
{
    public sealed class RequestValidationException : Exception
    {
        internal RequestValidationException()
        {
        }

        internal RequestValidationException(string message) : base(message)
        {
        }

        internal RequestValidationException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
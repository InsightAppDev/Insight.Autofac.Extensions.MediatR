using System;

namespace Insight.Autofac.Extensions.MediatR
{
    internal interface IHandlerDecorator
    {
        Type GetHandlerType();
    }
}
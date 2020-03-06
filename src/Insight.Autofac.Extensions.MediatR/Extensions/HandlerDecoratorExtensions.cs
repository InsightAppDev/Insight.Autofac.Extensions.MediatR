using System;

namespace Insight.Autofac.Extensions.MediatR
{
    internal static class HandlerDecoratorExtensions
    {
        internal static Type GetHandlerType(this object target)
        {
            return target is IHandlerDecorator decorator
                ? decorator.GetHandlerType()
                : target.GetType();
        }
    }
}
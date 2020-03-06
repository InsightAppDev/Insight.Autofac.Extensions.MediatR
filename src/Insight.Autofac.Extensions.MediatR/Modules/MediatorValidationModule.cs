using System;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using Module = Autofac.Module;

namespace Insight.Autofac.Extensions.MediatR
{
    internal sealed class MediatorValidationModule : Module
    {
        public Assembly[] Assemblies { get; }

        internal MediatorValidationModule(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                throw new ArgumentException("Empty assemblies!");
            
            Assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assemblies)
                .AsClosedTypesOf(typeof(AbstractValidator<>));

            builder.RegisterGenericDecorator(typeof(RequestValidationDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
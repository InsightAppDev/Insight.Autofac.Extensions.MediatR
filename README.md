Insight.Autofac.Extensions.MediatR
==================================
[![Build Status](https://travis-ci.org/InsightAppDev/Insight.Autofac.Extensions.MediatR.svg?branch=master)](https://travis-ci.org/InsightAppDev/Insight.Autofac.Extensions.MediatR)
[![nuget version](https://img.shields.io/nuget/v/Insight.Autofac.Extensions.MediatR)](https://www.nuget.org/packages/Insight.Autofac.Extensions.MediatR/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.Autofac.Extensions.MediatR?color=%2300000)](https://www.nuget.org/packages/Insight.Autofac.Extensions.MediatR/)

About
----------------------------------
This is infrastructure extensions which allow you easy use MediatR with Autofac. With this extensions you can register all your handlers from assemblies in autofac container by one line of code.

Logging decorator 
----------------------------------
By AddMediatorLogging() call you will register logging decorator which will generate logs for each executed command like this:

```
[VRB] 03-16 21:59:20 65652bfb-49c319305c8d0aa3 Insight.Autofac.Extensions.MediatR.Samples.Application.Queries.GetCurrentUtcDateTimeStringHandler Executing GetCurrentUtcDateTimeStringQuery:
{"Format": "yyyy-MM-dd", "$type": "GetCurrentUtcDateTimeStringQuery"}
```

Note that logging uses ILogger<> and requires registered ILoggerFactory and ILogger<>

Validation decorator
----------------------------------
By AddValidationDecorator(assemblies) call you will register all AbstractValidator<TCommand> from passed assemblies. They will be resolved in RequestValidationdecorator for TCommand as IEnumberable<AbstractValidator<TCommand>> and Validate() will be called for them. 
If at least one validator is not valid, an RequestValidationException will be thrown.

It's matters in which order you register decorators. First registered will be closer to the target handler. As example:
```csharp
var containerBuilder = new ContainerBuilder()
    .AddMediator("Insight.Autofac.Extensions.MediatR.Samples.Application")
    .AddMediatorValidation("Insight.Autofac.Extensions.MediatR.Samples.Application")
    .AddMediatorLogging();
```

It this case logging decorator will be executed before validation.

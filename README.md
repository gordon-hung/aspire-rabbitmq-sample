# aspire-rabbitmq-sample
ASP.NET Core 8.0 Aspire RabbitMQ

.NET Aspire is an opinionated, cloud ready stack for building observable, production ready, distributed applications. .NET Aspire is delivered through a collection of NuGet packages that handle specific cloud-native concerns. Cloud-native apps often consist of small, interconnected pieces or microservices rather than a single, monolithic code base. Cloud-native apps generally consume a large number of services, such as databases, messaging, and caching.

In this article, you learn how to use the .NET Aspire RabbitMQ client message-broker. The Aspire.RabbitMQ.Client library is used to register an IConnection in the dependency injection (DI) container for connecting to a RabbitMQ server. It enables corresponding health check, logging and telemetry.

## Get started
To get started with the .NET Aspire RabbitMQ integration, install the Aspire.RabbitMQ.Client NuGet package in the client-consuming project, i.e., the project for the application that uses the RabbitMQ client.
```sh
dotnet add package Aspire.RabbitMQ.Client
```

## App host usage
To model the RabbitMQ resource in the app host, install the Aspire.Hosting.RabbitMQ NuGet package in the app host project.
```sh
dotnet add package Aspire.Hosting.RabbitMQ
```

using Microsoft.AspNetCore.DataProtection;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var token = builder.AddParameter("token", secret: true);

var rabbitMQConnection = builder.AddRabbitMQ("RabbitMQConnection", username, password).WithManagementPlugin(port: 15672);

builder.AddProject<Projects.Aspire_RabbitMQSample_ClientApp>("aspire-rabbitmqsample-clientapp")
	.WithReference(rabbitMQConnection)
	.WithEnvironment("RABBITMQ_USERNAME", username)
	.WithEnvironment("RABBITMQ_PASSWORD", password)
	.WithEnvironment("RABBITMQ_TOKEN", token);

builder.AddProject<Projects.Aspire_RabbitMQSample_ServiceApp>("aspire-rabbitmqsample-serviceapp")
	.WithReference(rabbitMQConnection)
	.WithEnvironment("RABBITMQ_USERNAME", username)
	.WithEnvironment("RABBITMQ_PASSWORD", password)
	.WithEnvironment("RABBITMQ_TOKEN", token);

builder.Build().Run();

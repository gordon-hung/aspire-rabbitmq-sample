var builder = DistributedApplication.CreateBuilder(args);

var rabbitMQUsername = builder.AddParameter("RabbitMQUsername", secret: true);
var rabbitMQPassword = builder.AddParameter("RabbitMQPassword", secret: true);

var rabbitMQConnection = builder.AddRabbitMQ("RabbitMQConnection", rabbitMQUsername, rabbitMQPassword);

builder.AddProject<Projects.Aspire_RabbitMQSample_ClientApp>("aspire-rabbitmqsample-clientapp")
	.WithReference(rabbitMQConnection)
	.WithEnvironment("RabbitMQUsername", rabbitMQUsername)
	.WithEnvironment("RabbitMQPassword", rabbitMQPassword);
builder.AddProject<Projects.Aspire_RabbitMQSample_ServiceApp>("aspire-rabbitmqsample-serviceapp")
	.WithReference(rabbitMQConnection)
	.WithEnvironment("RabbitMQUsername", rabbitMQUsername)
	.WithEnvironment("RabbitMQPassword", rabbitMQPassword);

builder.Build().Run();

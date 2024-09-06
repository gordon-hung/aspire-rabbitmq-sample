using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;

namespace Aspire.RabbitMQSample.ServiceApp.Services;

public class RabbitMqHeadersAllBackgroundService(
	ILogger<RabbitMqHeadersAllBackgroundService> logger,
	IConfiguration configuration,
	IConnection connection) : BackgroundService
{
	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		string token = configuration.GetValue("RABBITMQ_TOKEN", "qwerASDFzxcv")!;

		return Task.Run(() =>
		{
			var channel = connection.CreateModel();
			var exchangeName = "amq.headers";
			var queueName = "amqp.headers.queue.all";
			var routingKeyName = "";

			//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
			channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

			var headers = new Dictionary<string, object>
			{
				{ "x-match", "all" }, // x-match: "all" or "any"
				{ "token", token },
				{ "event","all"}
			};
			channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKeyName, arguments: headers);

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, ea) =>
			{
				var data = new
				{
					LogAt = DateTimeOffset.UtcNow.ToString("O"),
					ea.Exchange,
					ea.RoutingKey,
					ea.BasicProperties.Headers,
					Message = JsonSerializer.Deserialize<object>(ea.Body.ToArray())
				};
				logger.LogInformation("{logInformation}", JsonSerializer.Serialize(data));
			};

			channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

			stoppingToken.Register(() =>
			{
				channel.Close();
				channel.Dispose();
			});

			// Prevents the service from exiting immediately
			Task.Delay(Timeout.Infinite, stoppingToken).Wait();
		}, stoppingToken);
	}
}

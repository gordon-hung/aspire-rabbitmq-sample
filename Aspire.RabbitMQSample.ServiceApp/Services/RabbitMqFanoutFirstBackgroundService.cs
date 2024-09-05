using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;

namespace Aspire.RabbitMQSample.ServiceApp.Services;

public class RabbitMqFanoutFirstBackgroundService(
    ILogger<RabbitMqFanoutFirstBackgroundService> logger,
    IConnection connection) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var channel = connection.CreateModel();
            var exchangeName = "amq.fanout";
            var queueName = "amqp.fanout.queue.first";
            var routingKeyName = string.Empty;//"amqp.fanout.group.first";

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKeyName);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var data = new
                {
                    LogAt = DateTimeOffset.UtcNow.ToString("O"),
                    ea.Exchange,
                    ea.RoutingKey,
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
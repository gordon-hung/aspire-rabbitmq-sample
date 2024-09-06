using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;

namespace Aspire.RabbitMQSample.ClientApp.Controllers;

[Route("[controller]")]
[ApiController]
public class DirectController(
	IConnection connection) : ControllerBase
{
	/// <summary>
	/// Directs the first asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("first")]
	public ValueTask DirectFirstAsync(
		[FromBody] string message = "direct-first")
	{
		string exchange = "amq.direct";
		string routingKey = "amqp.direct.routingKey.first";
		var body = JsonSerializer.SerializeToUtf8Bytes(new
		{
			LogAt = DateTime.UtcNow.ToString("u"),
			Message = message
		});

		using var channel = connection.CreateModel();
		channel.BasicPublish(exchange: exchange,
			routingKey: routingKey,
			basicProperties: null,
			body: body);
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Directs the second asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("second")]
	public ValueTask DirectSecondAsync(
		[FromBody] string message = "direct-second")
	{
		string exchange = "amq.direct";
		string routingKey = "amqp.direct.routingKey.second";
		var body = JsonSerializer.SerializeToUtf8Bytes(new
		{
			LogAt = DateTime.UtcNow.ToString("u"),
			Message = message
		});

		using var channel = connection.CreateModel();
		channel.BasicPublish(exchange: exchange,
			routingKey: routingKey,
			basicProperties: null,
			body: body);
		return ValueTask.CompletedTask;
	}
}

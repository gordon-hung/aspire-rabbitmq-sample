using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;

namespace Aspire.RabbitMQSample.ClientApp.Controllers;

[Route("[controller]")]
[ApiController]
public class TopicController(
	IConnection connection) : ControllerBase
{
	/// <summary>
	/// Topics the first asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("first")]
	public ValueTask TopicFirstAsync(
		[FromBody] string message = "topic-first")
	{
		string exchange = "amq.topic";
		string routingKey = "amqp.topic.routingkey.first";
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
	/// Topics the second asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("second")]
	public ValueTask TopicSecondAsync(
		[FromBody] string message = "topic-second")
	{
		string exchange = "amq.topic";
		string routingKey = "amqp.topic.routingkey.second";
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
	/// Topics the other asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("other")]
	public ValueTask TopicOtherAsync(
		[FromBody] string message = "topic-other")
	{
		string exchange = "amq.topic";
		string routingKey = "amqp.topic.other.other";
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
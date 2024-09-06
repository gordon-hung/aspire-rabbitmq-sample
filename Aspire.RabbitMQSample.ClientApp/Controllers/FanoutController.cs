using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;

namespace Aspire.RabbitMQSample.ClientApp.Controllers;

[Route("[controller]")]
[ApiController]
public class FanoutController(
	IConnection connection) : ControllerBase
{
	/// <summary>
	/// Fanouts the asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost]
	public ValueTask FanoutAsync(
		[FromBody] string message = "fanout")
	{
		string exchange = "amq.fanout";
		string routingKey = string.Empty;
		var body = JsonSerializer.SerializeToUtf8Bytes(new
		{
			LogAt = DateTime.UtcNow.ToString("u"),
			Message = message
		});

		using var channel = connection.CreateModel();
		channel.BasicPublish(
			exchange: exchange,
			routingKey: routingKey,
			basicProperties: null,
			body: body);
		return ValueTask.CompletedTask;
	}
}

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Aspire.RabbitMQSample.ClientApp.Controllers;
[Route("[controller]")]
[ApiController]
public class HeadersController(
	IConfiguration configuration,
	IConnection connection) : ControllerBase
{
	/// <summary>
	/// Headerses all asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("all")]
	public ValueTask HeadersAllAsync(
		[FromBody] string message = "headers-all")
	{
		string token = configuration.GetValue("RABBITMQ_TOKEN", "qwerASDFzxcv")!;
		string exchange = "amq.headers";
		string routingKey = string.Empty;
		var body = JsonSerializer.SerializeToUtf8Bytes(new
		{
			LogAt = DateTime.UtcNow.ToString("u"),
			Message = message
		});
		var headers = new Dictionary<string, object>
		{
			{ "x-match", "all" }, // x-match: "all" or "any"
            { "token",token },
			{ "event","all"}
		};

		using var channel = connection.CreateModel();
		var basicProperties = channel.CreateBasicProperties();
		basicProperties.Headers = headers;
		channel.BasicPublish(
			exchange: exchange,
			routingKey: routingKey,
			basicProperties: basicProperties,
			body: body);
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Headerses any asynchronous.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns></returns>
	[HttpPost("any")]
	public ValueTask HeadersAnyAsync(
		[FromBody] string message = "headers-any")
	{
		string token = configuration.GetValue("RABBITMQ_TOKEN", "qwerASDFzxcv")!;
		string exchange = "amq.headers";
		string routingKey = string.Empty;
		var body = JsonSerializer.SerializeToUtf8Bytes(new
		{
			LogAt = DateTime.UtcNow.ToString("u"),
			Message = message
		});
		var headers = new Dictionary<string, object>
		{
			{ "x-match", "any" }, // x-match: "all" or "any"
            { "token",token },
			{ "event","any"}
		};

		using var channel = connection.CreateModel();
		var basicProperties = channel.CreateBasicProperties();
		basicProperties.Headers = headers;
		channel.BasicPublish(
			exchange: exchange,
			routingKey: routingKey,
			basicProperties: basicProperties,
			body: body);
		return ValueTask.CompletedTask;
	}
}


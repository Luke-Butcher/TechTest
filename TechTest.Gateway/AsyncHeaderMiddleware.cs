using MassTransit;
using Common;
public class AsyncHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AsyncHeaderMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AsyncHeaderMiddleware(RequestDelegate next, ILogger<AsyncHeaderMiddleware> logger, IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post && context.Request.Headers.ContainsKey("X-Async"))
        {
            _logger.LogInformation("X-Async header detected, forwarding request to RabbitMQ.");

            // Read the request body
            context.Request.EnableBuffering();
            var bodyStream = new StreamReader(context.Request.Body);
            var bodyText = await bodyStream.ReadToEndAsync();
            context.Request.Body.Position = 0;

            // Create a message
            var message = new AsyncRequest
            {
                Path = context.Request.Path.ToString(),
                Headers = context.Request.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString()),
                Body = bodyText
            };

            // Create a scope to resolve IPublishEndpoint
            using (var scope = _serviceProvider.CreateScope())
            {
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                await publishEndpoint.Publish(message);
            }

            // Respond with a 202 Accepted status code
            context.Response.StatusCode = StatusCodes.Status202Accepted;
            await context.Response.WriteAsync("Request is being processed asynchronously.");
        }
        else
        {
            await _next(context);
        }
    }
}

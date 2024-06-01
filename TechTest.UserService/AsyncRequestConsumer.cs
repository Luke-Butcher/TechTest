using MassTransit;
using Common;

public class AsyncRequestConsumer : IConsumer<AsyncRequest>
{
    public async Task Consume(ConsumeContext<AsyncRequest> context)
    {
        var message = context.Message;
        // Process the asynchronous request here
        // For example, log the message or perform some action
        await Task.Run(() =>
        {
            Console.WriteLine($"Received async request for path: {message}");
        });
    }
}

using MassTransit;
using System.Threading.Tasks;

public class MyMessageConsumer : IConsumer<MyMessage>
{
    public async Task Consume(ConsumeContext<MyMessage> context)
    {
        var message = context.Message;

        var response = $"Received request for user: {message}";

        await context.RespondAsync(new MyResponse { Content = response });
    }
}

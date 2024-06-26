using MassTransit;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MyMessageConsumer>();
    x.AddConsumer<AsyncRequestConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // Read RabbitMQ connection settings from configuration
        var host = configuration["RabbitMQ:Host"];
        var username = configuration["RabbitMQ:Username"];
        var password = configuration["RabbitMQ:Password"];

        cfg.Host(host, "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });

        cfg.ReceiveEndpoint("users-queue", e =>
        {
            e.ConfigureConsumer<MyMessageConsumer>(context);
        });
        cfg.ReceiveEndpoint("async-requests-queue", e =>
        {
            e.ConfigureConsumer<AsyncRequestConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();
app.MapHub<placeholderSignalRHub>("/placeholderSignalRHub");

app.Run();

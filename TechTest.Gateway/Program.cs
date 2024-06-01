using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("OcelotConfig.json", false, true)
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"];
        var username = builder.Configuration["RabbitMQ:Username"];
        var password = builder.Configuration["RabbitMQ:Password"];

        cfg.Host(host, h =>
        {
            h.Username(username);
            h.Password(password);
        });
        cfg.ConfigureEndpoints(context);        

    });
});


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseMiddleware<AsyncHeaderMiddleware>();


app.UseOcelot().Wait();


app.MapControllers();


app.Run();


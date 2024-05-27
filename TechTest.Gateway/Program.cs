using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("OcelotConfig.json", false, true)
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration);
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

app.UseOcelot().Wait();

app.MapControllers();

app.Run();


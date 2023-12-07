using MassTransit;
using MassTransit.Producer.Configuration;
using MassTransit.Shared;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureMassTransit(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.MapPost("/order", async ([FromServices] IBus bus, [FromServices] MassTransitAzureConfigurationModel configurationModel) =>
{
    var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{configurationModel.QueueName}"));

    var user = new User(name: "Everton", email: "everton@email.com");
    var order = new Order(user: user);

    await endpoint.Send(order);

    return Results.Ok();
})
.WithName("Create Order")
.WithOpenApi()
.Produces<Order>();

app.Run();
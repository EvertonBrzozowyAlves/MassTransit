using MassTransit;
using MassTransit.Shared;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var server = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
var userName = configuration.GetSection("MassTransit")["UserName"] ?? string.Empty;
var password = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

builder.Services.AddMassTransit((options) =>
{
    options.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(server, "/", hostConfiguration =>
        {
            hostConfiguration.Username(userName);
            hostConfiguration.Password(password);
        });

        configuration.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.MapPost("/order", async ([FromServices] IBus bus, [FromServices] IConfiguration configuration) =>
{
    var queueName = configuration.GetSection("MassTransit")["QueueName"] ?? string.Empty;
    var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

    var user = new User(name: "Everton", email: "everton@email.com");
    var order = new Order(user: user);

    await endpoint.Send(order);

    return Results.Ok();
})
.WithName("Create Order")
.WithOpenApi()
.Produces<Order>(); ;

app.Run();